using Raspi.Temperature.App.Shared;
using System.Linq;
using Db = Raspi.Temperature.App.Server.Infrastructure.Model;

namespace Raspi.Temperature.App.Server.Extensions
{
    public static class DashboardExtensions
    {
        public static DeviceDashboard MapDeviceDashboard(this Db::Device source)
        {
            var from = source.TemperatureHistory.Any() ? source.TemperatureHistory.Min(_ => _.Date) : System.DateTime.MinValue;
            var to = source.TemperatureHistory.Any() ? source.TemperatureHistory.Max(_ => _.Date) : System.DateTime.MinValue;
            return new DeviceDashboard
            {
                DeviceId = source.Id,
                From = from,
                To = to,
                TemperatureHistory = source.TemperatureHistory.Select(_ => new TemperatureRecord
                {
                    Date = _.Date,
                    DegreeCelsius = _.DegreeCelsius
                }).ToArray()
            };
        }
    }
}
