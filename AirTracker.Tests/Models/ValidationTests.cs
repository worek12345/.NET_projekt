using Xunit;
using AirTracker.Models;

namespace AirTracker.Tests.Models
{
    public class ValidationTests
    {

        [Fact]
        public void Category_Name_Default_Is_Null()
        {
            var cat = new Category();
            Assert.Null(cat.Name);
        }

        [Fact]
        public void Sensor_Name_Can_Be_Assigned()
        {
            var sensor = new Sensor { Name = "AAA" };
            Assert.Equal("AAA", sensor.Name);
        }

        [Fact]
        public void Category_Name_Can_Be_Assigned()
        {
            var cat = new Category { Name = "TestCat" };
            Assert.Equal("TestCat", cat.Name);
        }
    }
}
