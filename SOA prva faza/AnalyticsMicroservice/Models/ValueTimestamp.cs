using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace AnalyticsMicroservice.Model
{
    public class ValueTimestamp
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SensorType { get; set; }
        public string Timestamp { get; set; }
        public double Value { get; set; }
        public ValueTimestamp(string sensorType, double value, string timestamp)
        {
            Timestamp = timestamp;
            Value = value;
            SensorType = sensorType;
        }
    }
}
