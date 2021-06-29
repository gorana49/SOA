using AnalyticsMicroservice.Model;
using MQTTnet;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;

namespace AnalyticsMicroservice.Services
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
                await _mqttService.Subscribe("sensor/data", OnDataReceived);
            }
        }
        private async void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                var bds = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                var des = System.Text.Json.JsonSerializer.Deserialize<SensorTimestamp>(bds);
                var options = new JsonSerializerOptions
                {
                   // PropertyNameCaseInsensitive = true,
                  //  PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                Console.WriteLine(des.SensorType);
                HttpClient httpClient = new HttpClient();
                var responseMessage = await httpClient.PostAsJsonAsync<SensorTimestamp>("http://192.168.100.22:8006/AnalyticsMicroservice", des,options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public async void PublishOnTopic(object data, string topic)
        {
            if(_mqttService.IsConnected())
            {
                await _mqttService.Publish(data, topic);
            }
        }
    }
}