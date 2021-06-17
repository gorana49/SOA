using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandMIcroservice.Hubs
{
    public class CommandHub : Hub
    {
        public async Task SendWarning(string SensorType,string message)
        {
            await Clients.All.SendAsync("SendCommand", SensorType, message);
        }
    }
}
