using CommandMIcroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandMIcroservice.IRepository
{
    public interface ICommandRepository
    {
        public Task PostData(SensorDataCommand sensorDataCommand);
        public Task<IEnumerable<SensorDataCommand>> GetData(string sensorType);
    }
}
