using Microsoft.EntityFrameworkCore;
using RS.Codeflix.Catalog.Infra.Data.EF;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
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

        public List<Category> GetExampleCategoriesListWithNames(List<string> names)
        {
            return names
                    .Select(name => {
                        var category = GetExampleCategory();
                        category.Update(name);
                        return category;
                    }).ToList();  
        }

        public List<Category> CloneCategoriesListOrdered(
            List<Category> categoriesList, 
            string orderBy, 
            SearchOrder order
        )
        {
            var listClone = new List<Category>(categoriesList);
            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name)
            };
            return orderedEnumerable.ToList();
        }
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
