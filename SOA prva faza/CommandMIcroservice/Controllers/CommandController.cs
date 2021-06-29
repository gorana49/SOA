using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandMIcroservice.Models;
using CommandMIcroservice.Services;
using System.Net.Http.Json;
using CommandMIcroservice.Hubs;

namespace CommandMIcroservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly CommandHub _commandHub;
        public CommandController(CommandHub commandHub)
        {
            _commandHub = commandHub;
        }
        [HttpPost]
        public async Task PostCommand([Required, FromBody] string command)
        {
            HttpClient httpClient = new HttpClient();
            if (command == "coolant")
            {
                Console.WriteLine("COOLANT NE DAJE DOBRE VREDNOSTI!");
                await  _commandHub.SendWarning("coolant", "Wrong value on sensor coolant!");
                var responseMessage = await httpClient.PostAsJsonAsync("http://coolant:80/api/Data/PostStop", command);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.Write("Uspelo!");
                }
            }
            else if (command == "pm" || command == "motor_speed")
            {
                await _commandHub.SendWarning("coolant", "Wrong value on sensor" + command);
                var responseMessage = await httpClient.PostAsJsonAsync("http://motor:80/api/Data/PostStop", command);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.Write("Uspelo!");
                }
            }
            else
            {
                await _commandHub.SendWarning("coolant", "Wrong value on sensor" + command);
                var responseMessage = await httpClient.PostAsJsonAsync("http://stator/api/Data/PostStop", command);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.Write("Uspelo!");
                }
            }
        }
    }
}
