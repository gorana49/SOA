using System;

namespace CoolingDeviceMicroservice.Models
{
    public class SensorData
    {
        public string SensorType { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
        public SensorData(double value, string sensorType)
        {
            this.Value = value;
            this.SensorType = sensorType;
            Timestamp = DateTime.Now;
        }
    }
}
