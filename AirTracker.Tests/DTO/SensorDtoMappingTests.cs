using Xunit;
using AirTracker.Models;
using AirTracker.DTO;

namespace AirTracker.Tests.DTO
{
    public class SensorDtoMappingTests
    {
        [Fact]
        public void Sensor_Maps_To_SensorDto_Correctly()
        {
            var sensor = new Sensor { Id = 1, Name = "Test", OpenAQSensorId = 123 };

            var dto = new SensorDto
            {
                Id = sensor.Id,
                Name = sensor.Name,
                OpenAQSensorId = sensor.OpenAQSensorId
            };

            Assert.Equal(sensor.Id, dto.Id);
            Assert.Equal(sensor.Name, dto.Name);
            Assert.Equal(sensor.OpenAQSensorId, dto.OpenAQSensorId);
        }
    }
}
