using DataMicroservice.Model;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Text;

namespace DataMicroservice.Services
{
    public class DataService
    {
        private Hivemq _mqttService;


        private event EventHandler ServiceCreated;
        public DataService(Hivemq mqttService)
        {
            _mqttService = mqttService;
        }

        public async void Publish(Sensor sensor)
        {
            while (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }
            if (_mqttService.IsConnected())
            {
                var data = new SensorTimestamp(sensor.SensorType, sensor.Value);
                await _mqttService.Publish(data, "sensor/data");
            }
        }


    }
}
