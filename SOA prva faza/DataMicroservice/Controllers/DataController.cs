using DataMicroservice.IRepository;
using DataMicroservice.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DataMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DataController : ControllerBase
    {
        public IDataRepository _dataRepository;
        public DataController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpPost]
        public Task Post([FromBody, Required] Sensor data)
        {
            var result = _dataRepository.PostData(data);
            return result;
        }

        [HttpGet("{sensorType}")]
        //public List<ValueTimestamp> GetData([FromRoute] string sensorType)
        public List<ValueTimestamp> GetData([FromRoute] string sensorType)
        {
            //List<ValueTimestamp> list = new List<ValueTimestamp>();
            List<ValueTimestamp> list = _dataRepository.GetData(sensorType);

            return list;
        }

        

    }
}
