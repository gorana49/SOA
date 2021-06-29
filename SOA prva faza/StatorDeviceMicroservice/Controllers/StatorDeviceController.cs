using Microsoft.AspNetCore.Mvc;
using StatorDeviceMicroservice.Models;
using StatorDeviceMicroservice.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace StatorDeviceMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StatorDeviceController : ControllerBase
    {
        private readonly ListOfSensorServices _listOfSensorService;
        public StatorDeviceController()
        {
            _listOfSensorService = new ListOfSensorServices();
        }

        [HttpGet("{type}")]
        public IActionResult GetSensorMetadata([Required, FromRoute] string type)
        {
            if (type == null)
                return BadRequest($"No sensor type specified.");

            foreach (SensorService sensor in this._listOfSensorService.listOfServices)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    SensorMetadata metadata = new SensorMetadata(type, sensor.Timeout.ToString(), sensor.Threshold.ToString());
                    return Ok(metadata);
                }
            }
            return BadRequest($"Sensor type: {type} doesn't exist!");
        }

        [HttpGet]
        public IActionResult GetAllSensorsParams()
        {

            return Ok(this._listOfSensorService);
        }

        [HttpGet("{type}")]
        public IActionResult GetTimeout([Required, FromRoute] string type)
        {
            foreach (var sensor in _listOfSensorService.listOfServices)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    string timeoutInfo = JsonSerializer.Serialize(new
                    {
                        isTimeout = !sensor.IsThresholdSet,
                        value = sensor.Timeout
                    }, options);

                    return Ok(timeoutInfo);
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpGet("{type}")]
        public IActionResult GetThreshold([Required, FromRoute] string type)
        {
            foreach (var sensor in _listOfSensorService.listOfServices)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    string tresholdInfo = JsonSerializer.Serialize(new { isTreshold = sensor.IsThresholdSet, value = sensor.Threshold }, options);
                    return Ok(tresholdInfo);
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult TurnOnOffSensor(string sensorType)
        {
            if(_listOfSensorService.listOfServices[0].SensorType == sensorType)
            {
                _listOfSensorService.listOfServices[0].SensorOn();
                return Ok($"Sensor {_listOfSensorService.listOfServices[0].SensorType} turned off");
            }
            else
            {
                _listOfSensorService.listOfServices[1].SensorOn();
                return Ok($"Sensor {_listOfSensorService.listOfServices[0].SensorType} turned off");
            }
            return Ok($"Sensor {_listOfSensorService.listOfServices[0].SensorType} alredy stopped");
        }

        [HttpPost("{type}")]
        public IActionResult SetTimeout(
            [Required, FromRoute] string type, [Required, FromBody] double? value)

        {
            foreach (var sensor in this._listOfSensorService.listOfServices)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    sensor.IsThresholdSet = false;
                    if (value != null)
                    {
                        sensor.SetTimeout((double)value);
                        return Ok($"Timeout based measuring started for {type} sensor. New Timeout value set");
                    }
                    else
                    {
                        return Ok($"Timeout based measuring started for {type} sensor. Last Timeout value used");
                    }
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

        [HttpPost("{type}")]
        public IActionResult SetThreshold(
            [Required, FromRoute] string type, [Required, FromBody] double? value)
        {
            if (value == null) return BadRequest("Provide treshold value");

            foreach (var sensor in _listOfSensorService.listOfServices)
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    sensor.IsThresholdSet = true;
                    if (value != null)
                    {
                        sensor.Threshold = (float)value;
                        return Ok($"Threshold based measuring started for {type} sensor. New Threshold value set");
                    }
                    else
                    {
                        return Ok($"Threshold based measuring started for {type} sensor. Default Threshold value used");
                    }
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }
        [HttpPost]
        public IActionResult PostStop(SensorData sensor)
        {
            if (sensor.SensorType == "stator_winding")
            {
                this._listOfSensorService.listOfServices[0].SensorOff();
                return Ok("stator_winding: turned off");
            }
            this._listOfSensorService.listOfServices[1].SensorOff();
            return Ok("stator_tooth: turned off");
        }
    }

}
