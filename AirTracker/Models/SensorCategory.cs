namespace AirTracker.Models
{
    public class SensorCategory
    {
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
