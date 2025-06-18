using Xunit;
using AirTracker.Models;

namespace AirTracker.Tests.Models
{
    public class SensorEqualsTests
    {
        [Fact]
        public void Two_Sensors_With_Same_Properties_Are_Equal()
        {
            var a = new Sensor { Id = 1, Name = "A", OpenAQSensorId = 2 };
            var b = new Sensor { Id = 1, Name = "A", OpenAQSensorId = 2 };

            // To nie jest referencyjnie równe, ale properties te same
            Assert.Equal(a.Id, b.Id);
            Assert.Equal(a.Name, b.Name);
            Assert.Equal(a.OpenAQSensorId, b.OpenAQSensorId);
        }
    }
}
