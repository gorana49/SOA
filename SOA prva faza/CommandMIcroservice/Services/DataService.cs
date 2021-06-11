using CommandMIcroservice.Models;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using CommandMIcroservice.IRepository;

namespace CommandMIcroservice.Services
{
    public class DataService
    {
        private Hivemq _mqttService;
        private readonly ICommandRepository _commandRepository;
        private event EventHandler ServiceCreated;
        public DataService(Hivemq mqttService, ICommandRepository repository)
        {
            _commandRepository = repository;
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
                await _mqttService.Subscribe("analytics", OnDataReceived);
                Console.WriteLine("Subscribed");
            }
        }

        private async void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                var json_data = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                SensorData sensorData = JsonConvert.DeserializeObject<SensorData>(json_data);
                SensorDataCommand sensorDataCommand = new SensorDataCommand(sensorData.SensorType, sensorData.Value, "out-of-range");
                await _commandRepository.PostData(sensorDataCommand);
                Console.WriteLine(sensorData);
                await this.SendToSensorsAsyncStop(sensorData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public async System.Threading.Tasks.Task SendToSensorsAsyncStop(SensorData sensorData)
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
