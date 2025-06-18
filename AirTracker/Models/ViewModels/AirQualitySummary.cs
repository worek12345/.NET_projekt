namespace AirTracker.Models.ViewModels
{
    public class AirQualitySummary
    {
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
