using System;

namespace Raspi.Temperature.App.Shared
{
    public class DeviceDashboard
    {
        public int DeviceId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public TemperatureRecord[] TemperatureHistory { get; set; }
    }
}
