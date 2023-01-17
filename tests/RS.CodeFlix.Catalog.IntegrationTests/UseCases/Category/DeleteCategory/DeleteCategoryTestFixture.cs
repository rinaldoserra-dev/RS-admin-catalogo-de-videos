using RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.Common;

namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }
    public class DeleteCategoryTestFixture
        : CategoryUsesCasesBaseFixture
    {
    }
}
