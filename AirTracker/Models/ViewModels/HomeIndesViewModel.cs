using System.Collections.Generic;
using AirTracker.Models;

namespace AirTracker.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Sensor> Sensors { get; set; } = new List<Sensor>();
        public int? SelectedSensorId { get; set; }
    }
}
