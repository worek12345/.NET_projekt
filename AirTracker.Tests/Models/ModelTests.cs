using Xunit;
using AirTracker.Models;

namespace AirTracker.Tests.Models
{
    public class ModelTests
    {
        [Fact]
        public void Sensor_Constructor_Works()
        {
            var sensor = new Sensor { Id = 3, Name = "Czujnik", OpenAQSensorId = 1111 };
            Assert.Equal("Czujnik", sensor.Name);
        }

        [Fact]
        public void Category_Constructor_Works()
        {
            var cat = new Category { Id = 9, Name = "Miasto" };
            Assert.Equal("Miasto", cat.Name);
        }
    }
}
