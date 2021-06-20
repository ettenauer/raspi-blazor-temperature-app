using System.Collections.Generic;

namespace Raspi.Temperature.App.Server.Infrastructure.Model
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Active { get; set; }

        public ICollection<TemperatureRecord> TemperatureHistory { get; set; }
    }
}
