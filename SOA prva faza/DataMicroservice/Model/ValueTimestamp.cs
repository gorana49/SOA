using System;

namespace DataMicroservice.Model
{
    public class ValueTimestamp
    {

        public ValueTimestamp(double value)
        {
            this.Timestamp = DateTime.Now;
            Value = value;
        }

        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
