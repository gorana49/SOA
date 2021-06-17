using DataMicroservice.Hubs;
using DataMicroservice.IRepository;
using DataMicroservice.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DataController : ControllerBase
    {
        private readonly DataHub _dataHub;
        public IDataRepository _dataRepository;
        public DataController(IDataRepository dataRepository, DataHub dataHub)
        {

            _dataHub = dataHub;
            _dataRepository = dataRepository;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Receive data from sensors", Description = "Data can be type of PM, SPEED_MOTOR,STATOR_WINDING")]
        [SwaggerResponse(200, "Everything is ok.")]
        [SwaggerResponse(204, "Something went wrong.")]
        public async Task<ActionResult<ValueTimestamp>> Post([FromBody] Sensor sensor)
        {
            await _dataHub.SendData(sensor.SensorType, sensor.Value);
            await _dataRepository.PostData(sensor);
            return StatusCode(200);
        }

        [HttpGet("{sensorType}")]
        [SwaggerOperation(Summary = "Returns all data from sensors", Description = "Data can be type of PM, SPEED_MOTOR,STATOR_WINDING")]
        [SwaggerResponse(200, "Everything is ok.")]
        [SwaggerResponse(204, "Something went wrong.")]
        public Task<IEnumerable<ValueTimestamp>> GetData([FromRoute] string sensorType)
        {
         
            var sensors = _dataRepository.GetData(sensorType);
            return sensors;
        }
    }
}
