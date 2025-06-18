using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirTracker.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    }
}
