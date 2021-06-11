using DataMicroservice.IRepository;
using DataMicroservice.Model;
using DataMicroservice.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DataMicroservice.Repository
{
    public class DataRepository : IDataRepository
    {
        private readonly ISensorContext _context;
        private readonly DataService _service;

        public DataRepository(ISensorContext context, DataService serv)
        {
            _context = context;
            _service = serv;
        }
        public async Task PostData(Sensor sensor)
        {
            ValueTimestamp vl = new ValueTimestamp(sensor.SensorType, sensor.Value);
            await _context.SensorData.InsertOneAsync(vl);
             _service.PublishOnTopic(sensor, "sensor/data");
        }
        public async Task<IEnumerable<ValueTimestamp>> GetData(string sensorType)
        {
            FilterDefinition<ValueTimestamp> filter = Builders<ValueTimestamp>.Filter.Eq(p => p.SensorType, sensorType);
            return await _context
                                        .SensorData
                                        .Find(filter)
                                        .ToListAsync();
        }
    }
}
