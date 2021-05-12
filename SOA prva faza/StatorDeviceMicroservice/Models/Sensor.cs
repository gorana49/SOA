using System;
namespace StatorDeviceMicroservice.Models
{
    [Serializable]
    public class Sensor
    {
        public string SensorType { get; set; }
        public double Value { get; set; }

        public Sensor(double value, string sensorType)
        {
            this.Value = value;
            this.SensorType = sensorType;
        }
    }
}
