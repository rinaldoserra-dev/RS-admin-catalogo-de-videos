using Bogus;
using Microsoft.EntityFrameworkCore;
using RS.CodeFlix.Catalog.Infra.Data.EF;

namespace RS.CodeFlix.Catalog.IntegrationTests.Base
{
    public class BaseFixture
    {
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
        }

        protected Faker Faker { get; set; }

        public CodeFlixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var dbContext = new CodeFlixCatalogDbContext(
                new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
            );
            if (!preserveData)
            {
                dbContext.Database.EnsureDeleted();
            }
            return dbContext;
        }
    }
}
