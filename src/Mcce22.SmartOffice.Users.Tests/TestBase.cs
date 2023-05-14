using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Users.Entities;

namespace Mcce22.SmartOffice.Users.Tests
{
    public abstract class TestBase
    {
        protected static IMapper Mapper { get; }

        protected static IdGenerator IdGenerator { get; }

        static TestBase()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(User).Assembly);
                cfg.AllowNullCollections = true;
            });

            Mapper = config.CreateMapper();
            IdGenerator = new IdGenerator();
        }
    }
}
