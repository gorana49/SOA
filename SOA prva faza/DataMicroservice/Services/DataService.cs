<<<<<<< HEAD
﻿using System;
using System.Text.Json;
=======
﻿using DataMicroservice.Model;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Text;

>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
namespace DataMicroservice.Services
{
    public class DataService
    {
        private Hivemq _mqttService;

<<<<<<< HEAD
=======

>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
        private event EventHandler ServiceCreated;
        public DataService(Hivemq mqttService)
        {
            _mqttService = mqttService;
<<<<<<< HEAD

            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);
        }

        private async void OnServiceCreated(object sender, EventArgs args)
=======
        }

        public async void Publish(Sensor sensor)
>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
        {
            while (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }
<<<<<<< HEAD
        }

        public async void PublishOnTopic(object data, string topic)
        {
            string jsonString = JsonSerializer.Serialize(data);
            await _mqttService.Publish(jsonString, topic);
        }
=======
            if (_mqttService.IsConnected())
            {
                var data = new SensorTimestamp(sensor.SensorType, sensor.Value);
                await _mqttService.Publish(data, "sensor/data");
            }
        }


>>>>>>> a0257b99e7c81c4f4783b3fb90aec616a8398df2
    }
}
