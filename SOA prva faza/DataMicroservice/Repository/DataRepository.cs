using DataMicroservice.IRepository;
using DataMicroservice.Model;
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
        public DataRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            ValueTimestamp vl = new ValueTimestamp(0);
            List<ValueTimestamp> list = new List<ValueTimestamp>();
            list.Add(vl);
            list.Add(vl);
            var serializedList = JsonConvert.SerializeObject(list);
            var redisList = Encoding.UTF8.GetBytes(serializedList);
            var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            //_distributedCache.SetAsync("coolant", redisList, options);
            _distributedCache.SetString("coolant", serializedList);
            _distributedCache.SetAsync("motor_speed", redisList, options);
            _distributedCache.SetAsync("pm", redisList, options);
            _distributedCache.SetAsync("stator_tooth", redisList, options);
            _distributedCache.SetAsync("stator_winding", redisList, options);
        }
        public async Task PostData(Sensor sensor)
        {
            /*ValueTimestamp vl = new ValueTimestamp(sensor.Value);
            var value = _distributedCache.Get("coolant");
            var serializedList = Encoding.UTF8.GetString(value);
            var list = JsonConvert.DeserializeObject<List<ValueTimestamp>>(serializedList);
            list.Add(vl);
            var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            var serl = JsonConvert.SerializeObject(list);
            var redisList = Encoding.UTF8.GetBytes(serl);
            await _distributedCache.SetAsync(sensor.SensorType, redisList, options);*/


            List<ValueTimestamp> list = GetData(sensor.SensorType);
            ValueTimestamp value = new ValueTimestamp(sensor.Value);
            list.Add(value);
            var jsonData1 = JsonConvert.SerializeObject(list);
            Console.WriteLine(jsonData1.ToString());
            //await _distributedCache.RemoveAsync(sensor.SensorType);
            await _distributedCache.RemoveAsync(sensor.SensorType);
            await _distributedCache.RefreshAsync(sensor.SensorType);
            await _distributedCache.SetStringAsync("coolant", jsonData1);


        }
        //public List<ValueTimestamp> GetData(string sensorType)
        public List<ValueTimestamp> GetData(string sensorType)
        {
            /*string serializedList;
            var list = new List<ValueTimestamp>();
            var redisList = _distributedCache.Get(sensorType);
            if (redisList != null)
            {
                serializedList = Encoding.UTF8.GetString(redisList);
                list = JsonConvert.DeserializeObject<List<ValueTimestamp>>(serializedList);
            }
            return list;*/

            var nesto = _distributedCache.GetString(sensorType);
            List<ValueTimestamp> lista = JsonConvert.DeserializeObject<List<ValueTimestamp>>(nesto);
            return lista;
        }
    }
}
