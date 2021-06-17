using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataMicroservice.Hubs
{
    public class DataHub : Hub
    {
        public async Task SendData(string SensorType, double Value)
        {
            await Clients.All.SendAsync("SendData", SensorType, Value);
        }
    }
}
