using DataMicroservice.Model;
using MongoDB.Driver;

#pragma warning disable CA1050 // Declare types in namespaces
public class SensorContext : ISensorContext
#pragma warning restore CA1050 // Declare types in namespaces
{
    public SensorContext()
    {
        var client = new MongoClient("mongodb://mongodbData:27017");
        var database = client.GetDatabase("SensorDataDb");
        SensorData = database.GetCollection<ValueTimestamp>("SensorData");
    }
    public IMongoCollection<ValueTimestamp> SensorData { get; }
}
