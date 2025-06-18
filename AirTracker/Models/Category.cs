namespace AirTracker.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Nawigacja: wiele Sensorów może mieć wiele Kategorii
        public ICollection<SensorCategory> SensorCategories { get; set; }
    }
}
