using CommandMIcroservice.Models;
using MongoDB.Driver;

public interface ISensorContext
{
    IMongoCollection<SensorDataCommand> SensorDataCommand { get; }
}
