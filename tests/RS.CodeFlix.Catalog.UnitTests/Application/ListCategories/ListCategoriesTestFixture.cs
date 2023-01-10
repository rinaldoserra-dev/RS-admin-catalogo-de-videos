using Moq;
using RS.CodeFlix.Catalog.Application.Interfaces;
using RS.CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using RS.CodeFlix.Catalog.UnitTests.Common;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection 
        : ICollectionFixture<ListCategoriesTestFixture>
    {}
    public class ListCategoriesTestFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetRepositoryMock()
        {
            return new Mock<ICategoryRepository>();
        }

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            return new Mock<IUnitOfWork>();
        }

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
            return (new Random().NextDouble() < 0.5);
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
            var list = new List<Category>();
            for (int i = 0; i < length; i++)
            {
                list.Add(GetExampleCategory());
            }

            return list;
        }

        public ListCategoriesInput getExampleInput()
        {
            var random = new Random();
            return new ListCategoriesInput(
                page: random.Next(1,10),
                perPage: random.Next(15, 100),
                search: Faker.Commerce.ProductName(),
                sort: Faker.Commerce.ProductName(),
                dir: random.Next(0,10) > 5 ? 
                    SearchOrder.Asc : SearchOrder.Desc
            );
        }
    }
}
