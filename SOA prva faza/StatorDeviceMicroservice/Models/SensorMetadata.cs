using System;
namespace StatorDeviceMicroservice.Models
{
    [Serializable]
    public class SensorMetadata
    {
        public string SensorType { get; set; }
        public string Timeout { get; set; }
        public string Treshold { get; set; }
        public SensorMetadata(string typesens, string timeoutSens, string Treshold)
        {
            SensorType = typesens;
            Timeout = timeoutSens;
            this.Treshold = Treshold;
        }
    }
}
