using Moq;
using RS.CodeFlix.Catalog.Application.Interfaces;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.Repository;
using RS.CodeFlix.Catalog.UnitTests.Common;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection
        : ICollectionFixture<DeleteCategoryTestFixture>
    { }

    public class DeleteCategoryTestFixture : BaseFixture
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

        public Category GetValidCategory()
        {
            return new Category(GetValidCategoryName(), GetValidCategoryDescription());
        }
    }
}
