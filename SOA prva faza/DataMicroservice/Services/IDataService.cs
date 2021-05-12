using DataMicroservice.Model;
using InfluxDB.Client.Writes;
namespace DataMicroservice.Services
{
    public interface IDataService
    {
        void Write(string data);
        void Write(PointData data);
        void SaveData(SensorData data);

    }
}
