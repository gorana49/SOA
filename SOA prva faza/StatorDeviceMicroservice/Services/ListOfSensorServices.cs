using System.Collections.Generic;

namespace StatorDeviceMicroservice.Services
{
    public class ListOfSensorServices
    {
        public List<SensorService> listOfServices { get; set; }

        public ListOfSensorServices()
        {
            this.listOfServices = new List<SensorService>
            {
                new SensorService("stator_winding"),
                new SensorService("stator_tooth")
            };
        }

        public void AddSensor(SensorService sensor)
        {
            this.listOfServices.Add(sensor);
        }
    }
}
