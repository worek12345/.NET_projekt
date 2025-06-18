using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirTracker.Models
{
    public class Sensor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int OpenAQSensorId { get; set; }

        // FK do Location – nullable, żeby migracja dodała kolumnę przy istniejących wierszach
        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<SensorCategory> SensorCategories { get; set; }
            = new List<SensorCategory>();
    }
}
