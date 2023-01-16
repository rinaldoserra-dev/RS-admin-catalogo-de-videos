﻿using FluentAssertions;
using RS.CodeFlix.Catalog.Infra.Data.EF;
using RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Repository = RS.CodeFlix.Catalog.Infra.Data.EF.Repositories;

namespace RS.CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [Collection(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTest
    {
        private readonly CategoryRepositoryTestFixture _fixture;

        public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Insert()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Insert(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Get()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList();
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

            var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory!.Id.Should().Be(exampleCategory.Id);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(GetNotFound))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task GetNotFound()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

            var dbCategoryNotFound = await categoryRepository.Get(Guid.NewGuid(), CancellationToken.None);

            dbCategoryNotFound.Should().BeNull();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Update()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var newCategoryValues = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
            await categoryRepository.Update(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(newCategoryValues.Name);
            dbCategory!.Id.Should().Be(exampleCategory.Id);
            dbCategory.Description.Should().Be(newCategoryValues.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Delete()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
           
            var exampleCategoriesList = _fixture.GetExampleCategoriesList();
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Delete(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true))
                .Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().BeNull();
        }

        [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnsListAndTotal()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();        
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);
            foreach(Category outputItem in output.Items) 
            {
                var exampleItem = exampleCategoriesList.Find(category => category.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SearchReturnsPaginated))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task SearchReturnsPaginated(
            int quantityCategoryToGenerate,
            int page,
            int perPage,
            int expectedQuantityItems
        )
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoryToGenerate);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(quantityCategoryToGenerate);
            output.Items.Should().HaveCount(expectedQuantityItems);
            foreach (Category outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(category => category.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 3, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        public async Task SearchByText(
            string search,
            int page,
            int perPage,
            int expectedQuantityItemsReturned,
            int expectedQuantityTotalItems
        )
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture
                .GetExampleCategoriesListWithNames(new List<string>
            {
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Based on Real Facts",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future"
            });
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);
            foreach (Category outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList
                    .Find(category => category.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchOrdered))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]
        [InlineData("", "desc")]
        public async Task SearchOrdered(
            string orderBy,
            string order
        )
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture
                .GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(exampleCategoriesList, orderBy, searchOrder);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);
            for (int i = 0; i < expectedOrderedList.Count; i++)
            {
                var expecteditem = expectedOrderedList[i];
                var outputItem = output.Items[i];
                expecteditem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(expecteditem!.Name);
                outputItem.Id.Should().Be(expecteditem!.Id);
                outputItem.Description.Should().Be(expecteditem.Description);
                outputItem.IsActive.Should().Be(expecteditem.IsActive);
                outputItem.CreatedAt.Should().Be(expecteditem.CreatedAt);
            }
        }
    }
}
