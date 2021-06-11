using DataMicroservice.IRepository;
using DataMicroservice.Model;
using DataMicroservice.Services;
<<<<<<< HEAD
using MongoDB.Driver;
=======
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DataMicroservice.Repository
{
    public class DataRepository : IDataRepository
    {
<<<<<<< HEAD
        private readonly ISensorContext _context;
        private readonly DataService _service;

        public DataRepository(ISensorContext context, DataService serv)
        {
            _context = context;
            _service = serv;
=======
        private readonly IDistributedCache _distributedCache;
        private readonly DataService _dataService;
        public DataRepository(IDistributedCache distributedCache, DataService dataService)
        {
            _distributedCache = distributedCache;
            _dataService = dataService;
>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
        }

        public void Initialize(Sensor sensor)
        {
            if (_distributedCache.Get(sensor.SensorType) == null)
            {
                
                List<ValueTimestamp> list = new List<ValueTimestamp>();
                var jsonData = JsonConvert.SerializeObject(list);
                _distributedCache.SetString(sensor.SensorType, jsonData);
            }
            
        }

        public async Task PostData(Sensor sensor)
        {
<<<<<<< HEAD
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
=======
            Initialize(sensor);


            List<ValueTimestamp> list = GetData(sensor.SensorType);
            ValueTimestamp value = new ValueTimestamp(sensor.Value);
            list.Add(value);
            var jsonData = JsonConvert.SerializeObject(list);
            await _distributedCache.SetStringAsync(sensor.SensorType, jsonData);

            _dataService.Publish(sensor);

        }
        public List<ValueTimestamp> GetData(string sensorType)
        {
            var nesto = _distributedCache.GetString(sensorType);
            List<ValueTimestamp> lista = JsonConvert.DeserializeObject<List<ValueTimestamp>>(nesto);
            return lista;
>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
        }

    }
}
