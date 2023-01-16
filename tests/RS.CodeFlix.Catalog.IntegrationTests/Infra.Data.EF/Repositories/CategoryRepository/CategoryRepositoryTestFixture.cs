using Microsoft.EntityFrameworkCore;
using RS.Codeflix.Catalog.Infra.Data.EF;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.IntegrationTests.Base;

namespace RS.CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTestFixtureCollection 
        : ICollectionFixture<CategoryRepositoryTestFixture>
    { }
    public class CategoryRepositoryTestFixture : BaseFixture
    {
        public string GetValidCategoryName()
        {
            var catergoryName = "";
            while (catergoryName.Length < 3)
                catergoryName = Faker.Commerce.Categories(1)[0];
            if (catergoryName.Length > 255)
                catergoryName = catergoryName[..255]; // catergoryName = catergoryName.Substring(0, 255);

            return catergoryName;
        }
        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }

        public bool GetRandomBoolean()
        {
            return new Random().NextDouble() < 0.5;
        }

        public Category GetExampleCategory()
        {
            return new Category(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean());
        }

        public List<Category> GetExampleCategoriesList(int length = 10)
        {
            return Enumerable.Range(1, length)
                    .Select(_ => GetExampleCategory())
                    .ToList();
        }

        public CodeflixCatalogDbContext CreateDbContext()
        {
            var dbContext = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
            );

            return dbContext;
        }
    }
}
