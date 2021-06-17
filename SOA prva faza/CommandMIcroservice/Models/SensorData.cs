using System;

namespace CommandMIcroservice.Models
{
    [Serializable]
    public class SensorData
    {
        public string SensorType { get; set; }
        public double Value { get; set; }
        public string Timestamp { get; set; }

        public SensorData(string sensorType, double value, string timestamp)
        {
            this.SensorType = sensorType;

            this.Value = value;

            this.Timestamp = timestamp;
        }
    }
}
