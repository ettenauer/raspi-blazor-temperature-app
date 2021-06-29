using Azure.Storage.Files.Shares;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Raspi.File.Importer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(configure => configure.AddConsole());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>()
                                        .CreateLogger<Program>();

            //Note: we use Environment Variable instead of arguments to prevent issue with Debugging 
            var config = ParseEnvironmentVaraiables();

            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            logger.LogInformation("Starting file importing ...");

            try
            {
                var shareService = new ShareServiceClient($"DefaultEndpointsProtocol=https;AccountName={config.StorageAccountName};AccountKey={config.StorageAccountKey};EndpointSuffix=core.windows.net");

                var confidentialClient = ConfidentialClientApplicationBuilder.Create(config.IdentityClientId)
                    .WithClientSecret(config.IdentityClientSecret)
                    .WithAuthority(new Uri(config.IdentityAuthority))
                    .Build();

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = await AcquireAccessTokenAsync(confidentialClient).ConfigureAwait(false);

                var share = shareService.GetShareClient("temperature");
                if (await share.ExistsAsync().ConfigureAwait(false))
                {
                    var dir = share.GetRootDirectoryClient();

                    await foreach (var item in dir.GetFilesAndDirectoriesAsync())
                    {
                        var file = dir.GetFileClient(item.Name);
                        var fileContent = await file.DownloadAsync().ConfigureAwait(false);

                        logger.LogInformation($"Import file {item.Name}");

                        foreach (var command in ReadDeviceCommands(fileContent.Value.Content))
                        {
                            try
                            {
                                var payload = JsonConvert.SerializeObject(command, jsonSettings);
                                await httpClient.PostAsync(new Uri(config.ImportApiUri), new StringContent(payload, Encoding.UTF8, "application/json"))
                                    .ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                //Note: not working command will be discarded and logged
                                logger.LogError(e, "Import call failed");
                            }
                        }

                        logger.LogInformation($"Import of file {item.Name} is finished");

                        dir.DeleteFile(item.Name);

                        logger.LogInformation($"File {item.Name} deleted");
                    }
                }
            }
            catch (Exception e)
            {
                //Note: catch all errors, programm will be retriggered again with next job schedule
                logger.LogError(e, "Import Job failed:");
            }
        }

        private static ImporterConfig ParseEnvironmentVaraiables()
        {
            var config = new ImporterConfig
            {
                StorageAccountName = Environment.GetEnvironmentVariable("STORAGEACCOUNTNAME"),
                StorageAccountKey = Environment.GetEnvironmentVariable("STORAGEACCOUNTKEY"),
                IdentityClientId = Environment.GetEnvironmentVariable("IDENTITYCLIENTID"),
                IdentityClientSecret = Environment.GetEnvironmentVariable("IDENTITYCLIENTSECRET"),
                IdentityAuthority = Environment.GetEnvironmentVariable("IDENTITYAUTHORITY"),
                ImportApiUri = Environment.GetEnvironmentVariable("IMPORTURI"),
            };

            if (!config.AllSet)
            {
                throw new ArgumentException("missing input parameter"); 
            }

            return config;
        }

        private static async Task<AuthenticationHeaderValue> AcquireAccessTokenAsync(IConfidentialClientApplication confidentialClient)
        {
            const string defaultScope = "https://ettenaueroutlook.onmicrosoft.com/RaspiTempApp/.default";
            var result = await confidentialClient.AcquireTokenForClient(new[] { defaultScope })
                                 .ExecuteAsync().ConfigureAwait(false);

            return new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }

        private static IEnumerable<NewTemperatureRecordCommand> ReadDeviceCommands(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ";"
            };
            using var csv = new CsvReader(reader, config);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                yield return new NewTemperatureRecordCommand
                {
                    Date = csv.GetField<DateTime>("Date"),
                    DegreeCelsius = csv.GetField<double>("DegreeCelsius"),
                    DeviceId = csv.GetField<int>("DeviceId")
                };
            }
        }
    }
}
