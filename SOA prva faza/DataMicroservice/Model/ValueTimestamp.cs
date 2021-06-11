using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace DataMicroservice.Model
{
    public class ValueTimestamp
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SensorType { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public ValueTimestamp(string sensorType, double value)
        {
            Timestamp = DateTime.Now;
            Value = value;
            SensorType = sensorType;
        }
    }
}
