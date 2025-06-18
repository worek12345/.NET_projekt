namespace AirTracker.Models.ViewModels
{
    public class SensorDetailsViewModel
    {
        public Sensor Sensor { get; set; } = default!;
        public IEnumerable<AirQuality> Data { get; set; } = new List<AirQuality>();
        public AirQualitySummary Summary { get; set; } = new AirQualitySummary();
    }
}
