using CsvHelper;
using CsvHelper.Configuration;
using StatorDeviceMicroservice.Models;
using System;
using System.Globalization;
using System.IO;
using System.Timers;
namespace StatorDeviceMicroservice.Services
{
    public class SensorService
    {

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

        public SensorService(string sensorType)
        {
            this.Threshold = DEFAULT_THRESHOLD; ;
            this.Timeout = 10000;
            _timer = new Timer(this.Timeout);
            _timer.Elapsed += OnTimerEvent;
            this.SensorType = sensorType;
            this._filePath = "C:\\Users\\Zeljko\\source\\repos\\measures_v2.csv";
            _timer.Start();
            this.IsOn = true;
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
            //if (!this.IsThresholdSet)
            //{
            //    Console.WriteLine($"{data.SensorType}: {data.Value}");
            //}
            //  if (sensor.Value > this.Threshold)
            //PUSHUJ NA DATA POST 

        }

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
