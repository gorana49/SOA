using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using AnalyticsMicroservice.IRepository;
using AnalyticsMicroservice.Services;
using AnalyticsMicroservice.Model; 
namespace AnalyticsMicroservice.Repository
{
    public class AnalyticsRepository: IAnalyticsRepository
    {
        private readonly ISensorContext _context;
        private readonly DataService _service;

        public AnalyticsRepository(ISensorContext context, DataService serv)
        {
            _context = context;
            _service = serv;
        }
        public async Task<IEnumerable<ValueTimestamp>> GetData(string sensorType)
        {
            FilterDefinition<ValueTimestamp> filter = Builders<ValueTimestamp>.Filter.Eq(p => p.SensorType, sensorType);
            return await _context
                                        .SensorData
                                        .Find(filter)
                                        .ToListAsync();
        }
        public async Task PostData(SensorTimestamp sensor)
        {
            ValueTimestamp vl = new ValueTimestamp(sensor.SensorType, sensor.Value, sensor.Timestamp);
            await _context.SensorData.InsertOneAsync(vl);
            
             _service.PublishOnTopic(sensor, "sensor/analytics");
        }
    }
}
