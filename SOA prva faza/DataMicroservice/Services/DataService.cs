using System;
using System.Text.Json;
namespace DataMicroservice.Services
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
        }

        public async void PublishOnTopic(object data, string topic)
        {
            string jsonString = JsonSerializer.Serialize(data);
            await _mqttService.Publish(jsonString, topic);
        }
    }
}
