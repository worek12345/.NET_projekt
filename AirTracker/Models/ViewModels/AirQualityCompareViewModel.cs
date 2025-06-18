using System.Collections.Generic;
using AirTracker.Models;

namespace AirTracker.Models.ViewModels
{
    public class AirQualityCompareViewModel
    {
        public string SensorName { get; set; }
        public Location Location1 { get; set; }
        public Location Location2 { get; set; }
        public List<AirQuality> Data1 { get; set; }
        public List<AirQuality> Data2 { get; set; }
    }
}
