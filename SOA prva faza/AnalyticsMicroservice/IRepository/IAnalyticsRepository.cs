using AnalyticsMicroservice.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalyticsMicroservice.IRepository
{
    public interface IAnalyticsRepository
    {
        Task<IEnumerable<ValueTimestamp>> GetData(string sensorType);
        Task PostData(SensorTimestamp sensor);
    }
}
