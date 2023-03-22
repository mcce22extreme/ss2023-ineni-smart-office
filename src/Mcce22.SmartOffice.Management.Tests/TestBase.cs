using AutoMapper;
using Mcce22.SmartOffice.Management.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Mcce22.SmartOffice.Management.Tests
{
    public abstract class TestBase
    {
        protected static IMapper Mapper { get; }

        protected static string UserName { get; } = Make.String();

        static TestBase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(User).Assembly);
                cfg.AllowNullCollections = true;
            });

            Mapper = config.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            var dbContext = CreateDbContext();

            // Reset in memory databsae
            dbContext.Database.EnsureDeleted();
        }

        protected AppDbContext CreateDbContext()
        {
            var dbOptionsBuilder = new DbContextOptionsBuilder();
            dbOptionsBuilder.UseInMemoryDatabase("testdb");

            var dbContext = new AppDbContext(dbOptionsBuilder.Options);

            return dbContext;
        }
    }
}
