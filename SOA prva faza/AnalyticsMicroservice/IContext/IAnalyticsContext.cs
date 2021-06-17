using AnalyticsMicroservice.Model;
using MongoDB.Driver;

public interface ISensorContext
{
    IMongoCollection<ValueTimestamp> SensorData { get; }
}
