using RS.CodeFlix.Catalog.UnitTests.Common;
using Xunit;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;

namespace RS.CodeFlix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTestFixture: BaseFixture
    {
        public CategoryTestFixture()
            : base() { }
        
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
            if(categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }

        public DomainEntity.Category GetValidCategory()
        {
            return new DomainEntity.Category(GetValidCategoryName(), GetValidCategoryDescription());
        }
    }

    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategoryTestFixtureCollection 
        : ICollectionFixture<CategoryTestFixture> { }
}
