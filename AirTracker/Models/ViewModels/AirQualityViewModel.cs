namespace AirTracker.Models.ViewModels
{
    public class AirQualityViewModel
    {
        public IEnumerable<Sensor> Sensors { get; set; } = new List<Sensor>();
        public int? SelectedSensorId { get; set; }
        public IEnumerable<AirQuality> Data { get; set; } = new List<AirQuality>();
        public AirQualitySummary Summary { get; set; } = new AirQualitySummary();
    }
}
