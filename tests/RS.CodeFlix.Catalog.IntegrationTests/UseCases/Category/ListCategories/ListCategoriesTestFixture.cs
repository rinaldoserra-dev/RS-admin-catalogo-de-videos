using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.Common;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture> { }
    public class ListCategoriesTestFixture
        : CategoryUsesCasesBaseFixture
    {
        public List<DomainEntity.Category> GetExampleCategoriesListWithNames(
            List<string> names
        ) => names.Select(name =>
        {
            var category = GetExampleCategory();
            category.Update(name);
            return category;
        }).ToList();

        public List<DomainEntity.Category> CloneCategoriesListOrdered(
            List<DomainEntity.Category> categoriesList,
            string orderBy,
            SearchOrder order
        )
        {
            var listClone = new List<DomainEntity.Category>(categoriesList);
            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
            };
            return orderedEnumerable.ToList();
        }
    }
}
