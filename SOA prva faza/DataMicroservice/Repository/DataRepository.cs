using DataMicroservice.IRepository;
using DataMicroservice.Model;
using DataMicroservice.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataMicroservice.Repository
{
    public class DataRepository : IDataRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly DataService _dataService;
        public DataRepository(IDistributedCache distributedCache, DataService dataService)
        {
            _distributedCache = distributedCache;
            _dataService = dataService;
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
        }

    }
}
