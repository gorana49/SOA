using DataMicroservice.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMicroservice.IRepository
{
    public interface IDataRepository
    {
        Task<IEnumerable<ValueTimestamp>> GetData(string sensorType);
        Task PostData(Sensor sensor);
    }
}
