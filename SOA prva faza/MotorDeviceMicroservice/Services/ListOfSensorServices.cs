using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
