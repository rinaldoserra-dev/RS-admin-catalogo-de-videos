using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;
using RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using RS.CodeFlix.Catalog.Infra.Data.EF.Repositories;
using RS.CodeFlix.Catalog.Infra.Data.EF;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
using ApplicationUseCase = RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RS.CodeFlix.Catalog.Flunt.Notifications;

namespace RS.CodeFlix.Catalog.IntegrationTests.UseCases.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 5,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(
            DomainEntity.Category exampleCategory,
            UpdateCategoryInput input
        )
        {
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCase.UpdateCategory(
                repository,
                unitOfWork
            );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 5,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutIsActive(
            DomainEntity.Category exampleCategory,
            UpdateCategoryInput exampleInput
        )
        {
            var input = new UpdateCategoryInput(
                exampleInput.Id, 
                exampleInput.Name, 
                exampleInput.Description
            );
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCase.UpdateCategory(
                repository,
                unitOfWork
            );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 5,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryOnlyName(
            DomainEntity.Category exampleCategory,
            UpdateCategoryInput exampleInput
        )
        {
            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name
            );
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackingInfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackingInfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCase.UpdateCategory(
                repository,
                unitOfWork
            );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(NotificationWhenCategoryNotFound))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]

        public async Task NotificationWhenCategoryNotFound()
        {
            var input = _fixture.GetValidInput();
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new ApplicationUseCase.UpdateCategory(
                repository,
                unitOfWork
            );

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().BeNull();
            useCase.Notifications.Should()
                .BeEquivalentTo(new List<Notification>()
                {
                    new Notification("Category", $"Category {input.Id} not found"),
                }
            );
        }

        [Theory(DisplayName = nameof(NofificationWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
            parameters: 6,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
        public async Task NofificationWhenCantInstantiateCategory(
            UpdateCategoryInput input,
            string PropertyMessage,
            string NotificationMessage
        )
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCategories = _fixture.GetExampleCategoriesList();
            await dbContext.AddRangeAsync(exampleCategories);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(
                repository,
                unitOfWork
            );
            input.Id = exampleCategories[0].Id;
            await useCase.Handle(input, CancellationToken.None);

            useCase.Valid.Should().BeFalse();
            useCase.Notifications.Should()
               .BeEquivalentTo(new List<Notification>() {
                new Notification(PropertyMessage, NotificationMessage)
               });
        }
    }
}
