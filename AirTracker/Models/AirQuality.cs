using System;

namespace AirTracker.Models
{
    public class AirQuality
    {
        public int SensorId { get; set; }
        public DateTime RetrievedAt { get; set; }

        // Surowa wartość (value)
        public double Value { get; set; }
        public string Parameter { get; set; } = string.Empty;
        public string Units { get; set; } = string.Empty;


        // Podsumowanie (summary)
        public double Min { get; set; }
        public double Q02 { get; set; }
        public double Q25 { get; set; }
        public double Median { get; set; }
        public double Q75 { get; set; }
        public double Q98 { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
        public double SD { get; set; }
    }
}
