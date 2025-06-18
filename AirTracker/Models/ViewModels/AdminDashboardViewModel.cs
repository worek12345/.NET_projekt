namespace AirTracker.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int LocationCount { get; set; }
        public int SensorCount { get; set; }
        public string MostUsedSensor { get; set; }  // <-- dodaj to
    }

}
