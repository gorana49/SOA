using DataMicroservice.Model;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using System;


namespace DataMicroservice.Services
{
    public class DataService
    {
        public const string DB_CONNECTION_URL = "http://influxdbData:8086";
        public const string DB_BUCKET = "soa";
        public const string DB_ORGANIZATION = "soa";
        public const string DB_USER = "soa";
        public readonly char[] DB_PASSWORD = "adminadmin".ToCharArray();

        private InfluxDBClient _client;

        public DataService()
        {
            _client = InfluxDBClientFactory.Create(DB_CONNECTION_URL, DB_USER, DB_PASSWORD);
        }

        public void Write(string data)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WriteRecord(DB_BUCKET, DB_ORGANIZATION, InfluxDB.Client.Api.Domain.WritePrecision.Ms, data);
            }
        }

        public void Write(PointData point)
        {
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WritePoint(DB_BUCKET, DB_ORGANIZATION, point);
            }
        }

        public void SaveData(SensorData sensorData)
        {
            var point = PointData
                      .Measurement("SensorsData")
                      .Tag("sensor", sensorData.SensorType)
                      .Field("value", sensorData.Value)
                      .Timestamp(DateTime.UtcNow, WritePrecision.Ms);
            using (WriteApi writeApi = _client.GetWriteApi())
            {
                writeApi.WritePoint(DB_BUCKET, DB_ORGANIZATION, point);
            }
        }
    }
}
