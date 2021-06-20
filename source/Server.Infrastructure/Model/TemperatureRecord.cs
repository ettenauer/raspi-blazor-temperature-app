using System;

namespace Raspi.Temperature.App.Server.Infrastructure.Model
{
    public class TemperatureRecord
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public double DegreeCelsius { get; set; }
    }
}
