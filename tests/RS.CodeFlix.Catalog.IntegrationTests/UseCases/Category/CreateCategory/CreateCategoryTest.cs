using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using RS.CodeFlix.Catalog.Infra.Data.EF;
using RS.CodeFlix.Catalog.Infra.Data.EF.Repositories;
using ApplicationUseCases = RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration/Application", "CreateCategoryTest - Use Cases")]
        public async Task CreateCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );

            var input = _fixture.GetInput();

            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Integration/Application", "CreateCategoryTest - Use Cases")]
        public async void CreateCategoryWithOnlyName()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );

            var input = new CreateCategoryInput(_fixture.GetInput().Name);

            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be("");
            dbCategory.IsActive.Should().Be(true);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().Be(true);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Integration/Application", "CreateCategoryTest - Use Cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );
            var exampleInput = _fixture.GetInput();
            var input = new CreateCategoryInput(exampleInput.Name, exampleInput.Description);

            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(true);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(true);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(NofificationWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "CreateCategoryTest - Use Cases")]
        [MemberData(
            nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
            parameters: 4,
            MemberType = typeof(CreateCategoryTestDataGenerator)
        )]
        public async void NofificationWhenCantInstantiateCategory(
            CreateCategoryInput input,
            string PropertyMessage,
            string NotificationMessage
        )
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );

            await useCase.Handle(input, CancellationToken.None);

            useCase.Valid.Should().BeFalse();
            useCase.Notifications.Should()
               .BeEquivalentTo(new List<Notification>() {
                new Notification(PropertyMessage, NotificationMessage)
               });
            var dbCategoriesList = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();
            dbCategoriesList.Should().HaveCount(0);
        }
    }
}
