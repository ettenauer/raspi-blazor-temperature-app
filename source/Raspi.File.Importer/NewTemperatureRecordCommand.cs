using System;

namespace Raspi.File.Importer
{
    public class NewTemperatureRecordCommand
    {
        public int DeviceId { get; init; }

        public DateTime Date { get; init; }

        public double DegreeCelsius { get; init; }
    }
}
