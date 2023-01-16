using RS.CodeFlix.Catalog.IntegrationTests.Base;
using RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }
    public class GetCategoryTestFixture : CategoryUsesCasesBaseFixture
    {
    }
}
