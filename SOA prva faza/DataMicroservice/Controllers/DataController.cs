using DataMicroservice.IRepository;
using DataMicroservice.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<ValueTimestamp>> Post([FromBody] Sensor sensor)
        {
            await _dataRepository.PostData(sensor);
            return StatusCode(200);
        }

        [HttpGet("{sensorType}")]
        public Task<IEnumerable<ValueTimestamp>> GetData([FromRoute] string sensorType)
        {
            var sensors = _dataRepository.GetData(sensorType);
            return sensors;
        }
    }
}
