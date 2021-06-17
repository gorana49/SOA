using CommandMIcroservice.Models;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace CommandMIcroservice.Services
{
    public class DataService
    {
        private Hivemq _mqttService;
        private event EventHandler ServiceCreated;
        public DataService(Hivemq mqttService)
        {
            _mqttService = mqttService;
            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);
        }
        private async void OnServiceCreated(object sender, EventArgs args)
        {
            while (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }
            if (_mqttService.IsConnected())
            {
                await _mqttService.Subscribe("sensor/analytics", OnDataReceived);
                Console.WriteLine("Subscribed");
            }
        }

        private async void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                var bds = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                var des = System.Text.Json.JsonSerializer.Deserialize<SensorTimestamp>(bds);
                Console.WriteLine(bds);
                Console.WriteLine(des.ToString());
                await this.SendToSensorsAsyncStop(des);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public async System.Threading.Tasks.Task SendToSensorsAsyncStop(SensorTimestamp sensorData)
        {

            HttpClient httpClient = new HttpClient();
            if (sensorData.SensorType == "coolant")
            {
                var responseMessage = await httpClient.PostAsJsonAsync("http://coolant/api/Data/PostStop", sensorData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.Write("Uspelo!");
                }
            }
            else if (sensorData.SensorType == "pm" || sensorData.SensorType == "motor_speed")
            {
                var responseMessage = await httpClient.PostAsJsonAsync("http://motor/api/Data/PostStop", sensorData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.Write("Uspelo!");
                }
            }
            else
            {
                var responseMessage = await httpClient.PostAsJsonAsync("http://stator/api/Data/PostStop", sensorData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.Write("Uspelo!");
                }
            }

        }
    }
}
