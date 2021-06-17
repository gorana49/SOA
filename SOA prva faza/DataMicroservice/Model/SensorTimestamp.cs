using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataMicroservice.Model
{
    [Serializable]
    public class SensorTimestamp
    {
        public string SensorType { get; set; }
        public double Value { get; set; }
        public string Timestamp { get; set; }

        public SensorTimestamp(string type, double value,  string vr)
        {
            this.SensorType = type;

            this.Value = value;

            this.Timestamp = vr;
        }
    }
}
