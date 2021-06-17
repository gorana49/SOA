using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsMicroservice.Model
{
    [Serializable]
    public class SensorTimestamp
    {

        public string SensorType { get; set; }
        public double Value { get; set; }
        public string Timestamp { get; set; }

        public SensorTimestamp(string sensorType, double value,  string timestamp)
        {
            this.SensorType = sensorType;

            this.Value = value;

            this.Timestamp = timestamp;
        }
    }
}
