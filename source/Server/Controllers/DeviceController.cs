using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Raspi.Temperature.App.Server.Commands;
using Raspi.Temperature.App.Server.Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Raspi.Temperature.App.Server.Controllers
{
    [ApiController]
    [RequiredScope("API.Access")]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IDbContextFactory<RaspiDbContext> dbContextFactory;

        public DeviceController(IDbContextFactory<RaspiDbContext> dbContextFactory, ILogger<DeviceController> logger)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut(Name = "NewTemperatureRecord")]
        public async Task<IActionResult> AddTemperatureRecordAsync(NewTemperatureRecordCommand newTemperatureRecord)
        {
            using var context = dbContextFactory.CreateDbContext();
            try
            {
                var device = await context.Devices
                                    .Include(_ => _.TemperatureHistory)
                                    .SingleOrDefaultAsync(_ => _.Id == newTemperatureRecord.DeviceId)
                                    .ConfigureAwait(false);

                if (device == null)
                    return BadRequest();

                device.TemperatureHistory.Add(new Infrastructure.Model.TemperatureRecord
                {
                    Date = newTemperatureRecord.Date,
                    DegreeCelsius = newTemperatureRecord.DegreeCelsius
                });

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to store {nameof(NewTemperatureRecordCommand)} for device {newTemperatureRecord.DeviceId}");
                throw;
            }

            return Ok();
        }
    }
}
