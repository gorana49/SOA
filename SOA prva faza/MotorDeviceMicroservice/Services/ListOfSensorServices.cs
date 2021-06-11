using System.Collections.Generic;

namespace MotorDeviceMicroservice.Services
{
    public class ListOfSensorServices

    {
        public List<SensorService> listOfServices { get; set; }

        public ListOfSensorServices()
        {
            this.listOfServices = new List<SensorService>
            {
                new SensorService("motor_speed"),
                new SensorService("pm")
            };
        }
    }
}
