﻿using RS.CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection
       : ICollectionFixture<GetCategoryTestFixture>
    { }
    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {

    }
}
