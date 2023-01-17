using FluentAssertions;
using RS.CodeFlix.Catalog.Infra.Data.EF.Repositories;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using ApplicationUseCase = RS.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;
        public GetCategoryTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Integration/Application", "GetCategory - Use Cases")]
        public async Task GetCategory()
        {
            var exampleCategory = _fixture.GetExampleCategory();
            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.Add(exampleCategory);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            
            var input = new ApplicationUseCase.GetCategoryInput(exampleCategory.Id);

            var useCase = new ApplicationUseCase.GetCategory(repository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(NotFoundNotificationWhenCategoryDoesntExist))]
        [Trait("Integration/Application", "GetCategory - Use Cases")]
        public async Task NotFoundNotificationWhenCategoryDoesntExist()
        {
            var exampleCategory = _fixture.GetExampleCategory();
            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.Add(exampleCategory);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);

            var input = new ApplicationUseCase.GetCategoryInput(Guid.NewGuid());

            var useCase = new ApplicationUseCase.GetCategory(repository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().BeNull();
            useCase.Notifications.Should()
                .BeEquivalentTo(new List<Notification>()
                {
                    new Notification("Category", $"Category {input.Id} not found"),
                }
            );
        }
    }
}
