using RS.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection
        : ICollectionFixture<DeleteCategoryTestFixture>
    { }

    public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
    {

    }
}
