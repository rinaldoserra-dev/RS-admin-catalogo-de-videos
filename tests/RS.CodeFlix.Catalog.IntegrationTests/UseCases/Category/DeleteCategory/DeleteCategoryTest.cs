using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RS.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using RS.CodeFlix.Catalog.Infra.Data.EF.Repositories;
using RS.CodeFlix.Catalog.Infra.Data.EF;
using ApplicationUseCase = RS.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using MediatR;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Integration/Application", "DeleteCategory - Use Cases")]
        public async Task DeleteCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var categoryExample = _fixture.GetExampleCategory();
            var exampleList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleList);
            var tracking = await dbContext.AddAsync(categoryExample);
            await dbContext.SaveChangesAsync();
            tracking.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.DeleteCategory(
                repository, unitOfWork
            );
            var input = new DeleteCategoryInput(categoryExample.Id);

            await useCase.Handle(input, CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var dbCategoryDeleted = await assertDbContext.Categories
                .FindAsync(categoryExample.Id);
            dbCategoryDeleted.Should().BeNull();
            var dbCategories = await assertDbContext
                .Categories.ToListAsync();
            dbCategories.Should().HaveCount(exampleList.Count);
        }

        [Fact(DisplayName = nameof(DeleteCategoryNotificationWhenNotFound))]
        [Trait("Integration/Application", "DeleteCategory - Use Cases")]
        public async Task DeleteCategoryNotificationWhenNotFound()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleList);
            await dbContext.SaveChangesAsync();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.DeleteCategory(
                repository, unitOfWork
            );
            var exampleGuid = Guid.NewGuid();
            var input = new DeleteCategoryInput(exampleGuid);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().Be(Unit.Value);
            useCase.Notifications.Should()
                .BeEquivalentTo(new List<Notification>()
                {
                    new Notification("Category", $"Category {exampleGuid} not found"),
                }
            );
        }
    }
}
