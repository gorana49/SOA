using Microsoft.AspNetCore.Mvc;
using MotorDeviceMicroservice.Services;
using System.ComponentModel.DataAnnotations;
using MotorDeviceMicroservice.Models;
using System.Text.Json;

namespace MotorDeviceMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoolingDeviceController : ControllerBase
    {
        private readonly SensorService _service;
        public CoolingDeviceController(SensorService service)
        {
            _service = service;
        }

        [HttpGet("{type}")]
        public IActionResult GetSensorMetadata([Required, FromRoute] string type)
        {
            if (type == null)
                return BadRequest($"No sensor type specified!");

            
            if (type.ToLower() == _service.SensorType.ToLower())
            {
                SensorMetadata metadata = new SensorMetadata(type, _service.Timeout.ToString(), _service.Threshold.ToString());

                return Ok(metadata);
            }
            
            return BadRequest($"Sensor type: {type} doesn't exist!");
        }


        [HttpGet("{type}")]
        public IActionResult GetTimeout([Required, FromRoute] string type)
        {
           if (type.ToLower() == _service.SensorType.ToLower())
           {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                string timeoutInfo = JsonSerializer.Serialize(new
                {
                    isTimeout = !_service.IsThresholdSet,
                    value = _service.Timeout
                }, options);

                return Ok(timeoutInfo);
           }
            
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpGet("{type}")]
        public IActionResult GetThreshold([Required, FromRoute] string type)
        {
            
            if (type.ToLower() == _service.SensorType.ToLower())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string tresholdInfo = JsonSerializer.Serialize(new { isTreshold = _service.IsThresholdSet, value = _service.Threshold }, options);
                return Ok(tresholdInfo);
            }
            
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult TurnOnOffSensor(
            [Required, FromBody] bool on, [Required, FromRoute] string type)
        {
            
            if (type.ToLower() == _service.SensorType.ToLower())
            {
                if (on)
                {
                    if (!_service.IsOn)
                    {
                        _service.SensorOn();
                        return Ok($"Sensor {type} turned on");
                    }
                    return Ok($"Sensor {type} alredy started");
                }
                else
                {
                    if (_service.IsOn)
                    {
                        _service.SensorOff();
                        return Ok($"Sensor {type} turned off");
                    }
                    return Ok($"Sensor {type} alredy stopped");
                }
            }
            
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult SetTimeout(
            [Required, FromRoute] string type, [Required, FromBody] double? value)

        {
            
            if (type.ToLower() == _service.SensorType.ToLower())
            {
                _service.IsThresholdSet = false;
                if (value != null)
                {
                    _service.SetTimeout((double)value);
                    return Ok($"Timeout based measuring started for {type} sensor. New Timeout value set");
                }
                else
                {
                    return Ok($"Timeout based measuring started for {type} sensor. Last Timeout value used");
                }
            }
            
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult SetThreshold(
            [Required, FromRoute] string type, [Required, FromBody] double? value)
        {
            if (value == null) return BadRequest("Provide treshold value");

            
            if (type.ToLower() == _service.SensorType.ToLower())
            {
                _service.IsThresholdSet = true;
                if (value != null)
                {
                    _service.Threshold = (float)value;
                    return Ok($"Threshold based measuring started for {type} sensor. New Threshold value set");
                }
                else
                {
                    return Ok($"Threshold based measuring started for {type} sensor. Default Threshold value used");
                }
            }
            
            return BadRequest("Type of sensor doesn't exist");
        }
    }
}
