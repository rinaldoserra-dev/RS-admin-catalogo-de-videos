using FluentAssertions;
using Moq;
using RS.CodeFlix.Catalog.Application.UseCases.Category.Common;
using RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
using UseCase = RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.UpdateCategory
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
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(
            DomainEntity.Category exampleCategory,
            UpdateCategoryInput input
        )
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            repositoryMock.Setup(x => x.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);
            repositoryMock.Verify(
                repository => repository.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            repositoryMock.Verify(
                repository => repository.Update(
                    exampleCategory,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            unitOfWorkMock.Verify(
               uow => uow.Commit(It.IsAny<CancellationToken>()),
               Times.Once
            );
        }

        [Fact(DisplayName = nameof(NotificationWhenCategoryNotFound))]
        [Trait("Application", "UpdateCategory - Use Cases")]

        public async Task NotificationWhenCategoryNotFound()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var input = _fixture.GetValidInput();

            repositoryMock.Setup(x => x.Get(
                    input.Id,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(() => null!);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Get(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);

            output.Should().BeNull();
            useCase.Notifications.Should()
                .BeEquivalentTo(new List<Notification>()
                {
                    new Notification("Category", $"Category {input.Id} not found"),
                }
            );
        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutProvidingIsActive(
            DomainEntity.Category exampleCategory,
            UpdateCategoryInput exampleInput
        )
        {
            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name,
                exampleInput.Description);
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            repositoryMock.Setup(x => x.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            repositoryMock.Verify(
                repository => repository.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            repositoryMock.Verify(
                repository => repository.Update(
                    exampleCategory,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            unitOfWorkMock.Verify(
               uow => uow.Commit(It.IsAny<CancellationToken>()),
               Times.Once
            );
        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryOnlyName(
            DomainEntity.Category exampleCategory,
            UpdateCategoryInput exampleInput
        )
        {
            var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name);
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            repositoryMock.Setup(x => x.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            repositoryMock.Verify(
                repository => repository.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            repositoryMock.Verify(
                repository => repository.Update(
                    exampleCategory,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
            unitOfWorkMock.Verify(
               uow => uow.Commit(It.IsAny<CancellationToken>()),
               Times.Once
            );
        }

        [Theory(DisplayName = nameof(NofificationWhenCantUpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
            parameters: 12,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
        public async Task NofificationWhenCantUpdateCategory(
            UpdateCategoryInput input,
            string PropertyMessage,
            string NotificationMessage
        )
        {
            var exampleCategory = _fixture.GetExampleCategory();
            input.Id = exampleCategory.Id;
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            repositoryMock.Setup(x => x.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);
            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            await useCase.Handle(input, CancellationToken.None);

            useCase.Valid.Should().BeFalse();
            useCase.Notifications.Should()
               .BeEquivalentTo(new List<Notification>() {
                new Notification(PropertyMessage, NotificationMessage)
               });
            repositoryMock.Verify(
                repository => repository.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);
        }
    }
}
