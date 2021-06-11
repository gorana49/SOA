using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandMIcroservice.Models;
using CommandMIcroservice.IRepository;
namespace CommandMicroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        public ICommandRepository _commandrepository;
        public CommandController(ICommandRepository commandRepository)
        {
            _commandrepository = commandRepository;
        }
        [HttpGet("{sensorType}")]
        public Task<IEnumerable<SensorDataCommand>> GetData([FromRoute] string sensorType)
        {
            var sensors = _commandrepository.GetData(sensorType);
            return sensors;
        }
    }
}
