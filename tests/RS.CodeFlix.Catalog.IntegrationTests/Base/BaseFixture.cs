using Bogus;
using Microsoft.EntityFrameworkCore;
using RS.Codeflix.Catalog.Infra.Data.EF;

namespace RS.CodeFlix.Catalog.IntegrationTests.Base
{
    public class BaseFixture
    {
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
        }

        protected Faker Faker { get; set; }

        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var dbContext = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
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
