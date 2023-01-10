using RS.CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using RS.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection
        : ICollectionFixture<ListCategoriesTestFixture>
    { }
    public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
    {

        public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
        {
            var list = new List<DomainEntity.Category>();
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
                page: random.Next(1, 10),
                perPage: random.Next(15, 100),
                search: Faker.Commerce.ProductName(),
                sort: Faker.Commerce.ProductName(),
                dir: random.Next(0, 10) > 5 ?
                    SearchOrder.Asc : SearchOrder.Desc
            );
        }
    }
}
