using System.Collections.Generic;
using AirTracker.Models;

namespace AirTracker.Models.ViewModels
{
    public class AirQualitySelectionViewModel
    {
        public List<string> Cities { get; set; } = new();
        public string? SelectedCity { get; set; }

        public List<Location> Locations { get; set; } = new();
        public int? SelectedLocationId { get; set; }

        public List<Sensor> Sensors { get; set; } = new();
        public int? SelectedSensorId { get; set; }

        public List<AirQuality> Data { get; set; } = new();
        public List<Location> AllLocations { get; set; } = new();

    }
}
