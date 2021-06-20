using System;

namespace Raspi.Temperature.App.Server.Commands
{
    //Note: can be moved to differend project, but is not intended to share commans with App.Client
    public class NewTemperatureRecordCommand
    {
        public int DeviceId { get; init; }

        public DateTime Date { get; init; }

        public double DegreeCelsius { get; init; }
    }
}
