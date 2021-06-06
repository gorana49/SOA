using CsvHelper;
using CsvHelper.Configuration;
using StatorDeviceMicroservice.Models;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Timers;

namespace StatorDeviceMicroservice.Services
{
    public class SensorService
    {
        //private static readonly HttpClient client = new HttpClient();
        // private readonly IHttpClientFactory _clientFactory;
        public const float DEFAULT_THRESHOLD = 2500;
        public float Threshold { get; set; }
        public double Timeout { get; set; }
        public bool IsThresholdSet { get; set; }
        public bool IsOn { get; set; }
        public double Value { get; set; }
        public string SensorType { get; set; }
        public string _filePath;

        private readonly Timer _timer;
        private StreamReader _streamReader;
        private CsvReader _csv;

        public SensorService(string sensorType)//, IHttpClientFactory fac)
        {
            this.Threshold = DEFAULT_THRESHOLD; ;
            this.Timeout = 10000;
            _timer = new Timer(this.Timeout);
            _timer.Elapsed += OnTimerEvent;
            this.SensorType = sensorType;
            this._filePath = "/SOA/measures_v2.csv";
            this.IsOn = false;
            this.setCsv();
            this.IsThresholdSet = false;
        }
        public void SensorOff()
        {
            IsOn = false;
            _timer.Stop();
        }

        public void SensorOn()
        {
            IsOn = true;
            _timer.Start();
        }
        private void setCsv()
        {
            this._streamReader = new StreamReader(this._filePath);
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            this._csv = new CsvReader(_streamReader, config);
            _csv.Read();
            _csv.ReadHeader();
        }

        private async void OnTimerEvent(object sender, ElapsedEventArgs args)
        {
            this.ReadValue();
            Sensor sensor = new Sensor(this.Value, this.SensorType);
            HttpClient httpClient = new HttpClient();
            var responseMessage = await httpClient.PostAsJsonAsync("http://data/api/Data/Post", sensor);
        }
        //private async Task SendValueAsync()
        //{
        //    if (IsTreshold)
        //    {
        //        if (Value > TresholdValue)
        //            await PostRequest("http://swagger_dataservice_1/api/Data/AddData");
        //    }
        //    else
        //    {
        //        await PostRequest("http://swagger_dataservice_1/api/Data/AddData");
        //    }
        //}

        //private async Task PostRequest(string uri)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    string data = System.Text.Json.JsonSerializer.Serialize(new
        //    {
        //        RecordTime = DateTime.Now.ToShortTimeString(),
        //        SensorType = Type,
        //        Value
        //    });

        //    Console.WriteLine(data);

        //    try
        //    {
        //        await httpClient.PostAsync(uri, new StringContent(
        //            System.Text.Json.JsonSerializer.Serialize(new
        //            {
        //                RecordTime = DateTime.Now.ToShortTimeString(),
        //                SensorType = Type,
        //                Value
        //            }
        //        ), Encoding.UTF8, "application/json"));
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}
        public void SetTimeout(double interval)
        {
            _timer.Stop();
            Timeout = interval;
            _timer.Interval = interval;
            _timer.Start();
        }


        private void ReadValue()
        {
            try
            {
                string sensor_value;
                if (_csv.Read())
                    sensor_value = _csv.GetField<string>(this.SensorType);
                else
                {
                    _streamReader.DiscardBufferedData();
                    using (this._csv) { }
                    this.setCsv();
                    _csv.Read();
                    sensor_value = _csv.GetField<string>(this.SensorType);
                }
                this.Value = double.Parse(sensor_value, CultureInfo.InvariantCulture);
            }
            catch (IOException e)
            {
                Console.WriteLine("This file couldn't be read:");
                Console.WriteLine(e.StackTrace);
            }
        }

    }
}
