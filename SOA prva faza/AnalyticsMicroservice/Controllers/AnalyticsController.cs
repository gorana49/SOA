using AnalyticsMicroservice.IRepository;
using AnalyticsMicroservice.Model;
using AnalyticsMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AnalyticsController:ControllerBase
    {
        public IAnalyticsRepository _dataRepository;
        public AnalyticsController(IAnalyticsRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        [HttpPost]
        public async Task<StatusCodeResult> Post([FromBody] SensorEvent sensor)
        {
            SensorTimestamp st = sensor.Event;
            await _dataRepository.PostData(st);
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
