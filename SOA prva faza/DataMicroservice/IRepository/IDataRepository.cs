using DataMicroservice.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMicroservice.IRepository
{
    public interface IDataRepository
    {
        public Task PostData(Sensor sensor);
        public List<ValueTimestamp> GetData(string sensorType);
    }
}
