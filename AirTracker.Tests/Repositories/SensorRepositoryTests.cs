using AirTracker.Models;
using AirTracker.DTO;
using AirTracker.Data;
using AirTracker.Repositories;
using Microsoft.EntityFrameworkCore;



namespace AirTracker.Tests.Repositories
{
    public class SensorRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("RepoTestDb")
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Repo_Add_And_Find_Sensor_Works()
        {
            var ctx = GetDbContext();
            var repo = new SensorRepository(ctx);

            var sensor = new Sensor { Name = "Test", OpenAQSensorId = 42 };
            await repo.AddAsync(sensor);

            var found = await repo.GetByIdAsync(sensor.Id);
            Assert.NotNull(found);
            Assert.Equal("Test", found.Name);
        }
    }
}
