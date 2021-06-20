using Raspi.Temperature.App.Shared;
using System.Collections.Generic;
using System.Linq;
using Db = Raspi.Temperature.App.Server.Infrastructure.Model;

namespace Raspi.Temperature.App.Server.Extensions
{
    public static class DeviceExtensions
    {
        public static IEnumerable<Device> MapDevices(this IEnumerable<Db::Device> source)
        {
            return source.Select(_ => _.MapDevice());
        }

        public static Device MapDevice(this Db::Device source)
        {
            return new Device
            {
                Id = source.Id,
                Name = source.Name,
                Latitude = source.Latitude,
                Longitude = source.Longitude
            };
        }
    }
}
