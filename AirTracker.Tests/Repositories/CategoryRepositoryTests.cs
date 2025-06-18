using Xunit;
using AirTracker.Models;
using AirTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace AirTracker.Tests.Repositories
{
    public class CategoryRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDbCategories")
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Add_And_Get_Category_Works()
        {
            var ctx = GetDbContext();
            ctx.Categories.Add(new Category { Name = "TestCat" });
            await ctx.SaveChangesAsync();

            var cats = await ctx.Categories.ToListAsync();
            Assert.Single(cats);
            Assert.Equal("TestCat", cats.First().Name);
        }

        [Fact]
        public async Task Delete_Category_Works()
        {
            var ctx = GetDbContext();
            var cat = new Category { Name = "ToDelete" };
            ctx.Categories.Add(cat);
            await ctx.SaveChangesAsync();

            ctx.Categories.Remove(cat);
            await ctx.SaveChangesAsync();

            Assert.Empty(ctx.Categories.ToList());
        }
    }
}
