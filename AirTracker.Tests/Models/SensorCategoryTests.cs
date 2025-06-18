using Xunit;
using AirTracker.Models;
using System.Collections.Generic;

namespace AirTracker.Tests.Models
{
    public class SensorCategoryTests
    {
        [Fact]
        public void Can_Assign_ManyToMany_Relation()
        {
            var sensor = new Sensor { Id = 1, Name = "Test", OpenAQSensorId = 42, SensorCategories = new List<SensorCategory>() };
            var cat = new Category { Id = 1, Name = "City", SensorCategories = new List<SensorCategory>() };

            var join = new SensorCategory { Sensor = sensor, SensorId = sensor.Id, Category = cat, CategoryId = cat.Id };

            sensor.SensorCategories.Add(join);
            cat.SensorCategories.Add(join);

            Assert.Single(sensor.SensorCategories);
            Assert.Single(cat.SensorCategories);
            Assert.Equal(sensor.Id, join.SensorId);
            Assert.Equal(cat.Id, join.CategoryId);
        }
    }
}
