using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace GatewayMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private HttpClient _httpClient;

        public GatewayController()
        {
            this._httpClient = new HttpClient();
        }

        private async Task<ContentResult> ProxyPost(string url, string jsonBody)
        {
            var responseMessage = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            return Content(await responseMessage.Content.ReadAsStringAsync());
        }
        private async Task<ContentResult> ProxyGet(string url)
            => Content(await _httpClient.GetStringAsync(url));

        [HttpGet("{type}")]
        public async Task<IActionResult> GetSensorParams([FromRoute] string type)
        {
            if (type.StartsWith("stator"))
            {
                await ProxyGet("http://api/StatorDevice/GetSensorParams/" + type);
            }
            else if (type.StartsWith('c'))
            {
                await ProxyGet("http://api/CoolingDevice/GetSensorParams/" + type);
            }
            else
            {
                await ProxyGet("http://api/MotorDevice/GetSensorParams/" + type);
            }
            return BadRequest("Sensor is off.");
        }



        [HttpGet("{type}")]
        public async Task<IActionResult> GetAllSensorsParams([FromRoute] string type)
        {
            if (type.StartsWith("stator"))
            {
                await ProxyGet("http://api/StatorDevice/GetAllSensorsParams/");
            }
            else if (type.StartsWith('c'))
            {
                await ProxyGet("http://api/CoolingDevice/GetAllSensorsParams/");
            }
            else
            {
                await ProxyGet("http://api/MotorDevice/GetAllSensorsParams/");
            }
            return BadRequest("Sensors are disconnected.");
        }


        [HttpGet("{type}")]
        public async Task<IActionResult> GetTimeout([FromRoute] string type)
        {
            if (type.StartsWith("stator"))
            {
                await ProxyGet("http://api/StatorDevice/GetTimeout/" + type);
            }
            else if (type.StartsWith('c'))
            {
                await ProxyGet("http://api/CoolingDevice/GetTimeout/" + type);
            }
            else
            {
                await ProxyGet("http://api/MotorDevice/GetTimeout/" + type);
            }
            return BadRequest("Sensors are disconnected.");
        }


        [HttpGet("{type}")]
        public async Task<IActionResult> GetThreshold([FromRoute] string type)
        {
            if (type.StartsWith("stator"))
            {
                await ProxyGet("http://api/StatorDevice/GetThreshold/" + type);
            }
            else if (type.StartsWith('c'))
            {
                await ProxyGet("http://api/CoolingDevice/GetThreshold/" + type);
            }
            else
            {
                await ProxyGet("http://api/MotorDevice/GetThreshold/" + type);
            }
            return BadRequest("Sensors are disconnected.");
        }


        //[HttpPost("{type}")]
        //public async Task<IActionResult> TurnOnOffSensor([FromRoute] string type, [FromBody] bool on)
        //{
        //    if (type.StartsWith("stator"))
        //    {
        //        await ProxyPost("http://api/StatorDevice/TurnOnOffSensor/" + type, JsonSerializer.Serialize(on, typeof(bool)));
        //    }
        //    else if (type.StartsWith('c'))
        //    {
        //        await ProxyPost("http://api/CoolingDevice/TurnOnOffSensor/" + type, JsonSerializer.Serialize(on, typeof(bool)));
        //    }
        //    else
        //    {
        //        await ProxyPost("http://api/MotorDevice/TurnOnOffSensor/" + type, JsonSerializer.Serialize(on, typeof(bool)));
        //    }
        //    return BadRequest("Sensors are disconnected.");
        //}


        //[HttpPost("{type}")]
        //public async Task<IActionResult> SetTimeout(
        //    [Required, FromRoute] string type, [FromQuery(Name = "value")] double? value)
        //    => await ProxyPost("http://device/api/Device/SetTimeout/" + type, JsonSerializer.Serialize(value, typeof(double)));

        //[HttpPost("{type}")]
        //public async Task<IActionResult> SetThreshold(
        //    [Required, FromRoute] string type, [Required, FromQuery(Name = "value")] double? value)
        //    => await ProxyPost("http://device/api/Device/SetThreshold/" + type, JsonSerializer.Serialize(value, typeof(double)));

        ////DATA ENDPOINTS
        //[HttpGet("{type}")]
        //async public Task<IActionResult> GetSensorCurrentValue([Required, FromRoute] string type)
        //    => await ProxyGet("http://data/api/Data/GetSensorCurrentValue/" + type);

        //[HttpGet]
        //async public Task<IActionResult> GetAllSensorsCurrentValues()
        //    => await ProxyGet("http://data/api/Data/GetAllSensorsCurrentValues");

        //[HttpGet("{type}")]
        //async public Task<IActionResult> GetMaxValue([Required, FromRoute] string type)
        //    => await ProxyGet("http://data/api/Data/GetMaxValue/" + type);

        //[HttpGet]
        //async public Task<IActionResult> GetAllSensorsMaxValues()
        //    => await ProxyGet("http://data/api/Data/GetAllSensorsMaxValues");

        //[HttpGet("{type}")]
        //async public Task<IActionResult> GetMinValue([Required, FromRoute] string type)
        //    => await ProxyGet("http://data/api/Data/GetMinValue/" + type);

        //[HttpGet]
        //async public Task<IActionResult> GetAllSensorsMinValues()
        //    => await ProxyGet("http://data/api/Data/GetAllSensorsMinValues");

        //[HttpGet("{type}")]
        //async public Task<IActionResult> GetLastNHoursMeanValue([Required, FromRoute] string type, [Required, FromQuery(Name = "hours")] int? hours)
        //    => await ProxyGet("http://data/api/Data/GetLastNHoursMeanValue/" + type + "?hours=" + hours.ToString());

        //[HttpGet]
        //async public Task<IActionResult> GetAllSensorsLastNHoursMeanValues([Required, FromQuery(Name = "hours")] int? hours)
        //    => await ProxyGet("http://data/api/Data/GetAllSensorsLastNHoursMeanValues" + "?hours=" + hours.ToString());

        //[HttpGet("{type}")]
        //async public Task<IActionResult> GetLastNMinutesValues([Required, FromRoute] string type, [Required, FromQuery(Name = "minutes")] int? minutes)
        //    => await ProxyGet("http://data/api/Data/GetLastNMinutesValues/" + type + "?minutes=" + minutes.ToString());

        //[HttpGet]
        //async public Task<IActionResult> GetAllSensorsLastNMinutesValues([Required, FromQuery(Name = "minutes")] int? minutes)
        //    => await ProxyGet("http://data/api/Data/GetAllSensorsLastNMinutesValues" + "?minutes=" + minutes.ToString());

    }
}
