using Xunit;
using AirTracker.Models;
using System.Collections.Generic;

namespace AirTracker.Tests.Models
{
    public class SensorCategoryUnlinkTests
    {
        [Fact]
        public void Can_Remove_Relation_SensorCategory()
        {
            var sensor = new Sensor { Id = 2, Name = "Test", OpenAQSensorId = 42, SensorCategories = new List<SensorCategory>() };
            var cat = new Category { Id = 3, Name = "Alert", SensorCategories = new List<SensorCategory>() };
            var link = new SensorCategory { Sensor = sensor, SensorId = sensor.Id, Category = cat, CategoryId = cat.Id };

            sensor.SensorCategories.Add(link);
            cat.SensorCategories.Add(link);

            // Teraz usuwamy powiązanie
            sensor.SensorCategories.Remove(link);
            cat.SensorCategories.Remove(link);

            Assert.Empty(sensor.SensorCategories);
            Assert.Empty(cat.SensorCategories);
        }
    }
}
