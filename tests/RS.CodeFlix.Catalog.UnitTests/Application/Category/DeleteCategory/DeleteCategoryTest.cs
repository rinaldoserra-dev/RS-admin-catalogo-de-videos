using FluentAssertions;
using MediatR;
using Moq;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;
using UseCase = RS.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory
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
        [Trait("Application", "DeleteCategoryTest - Use Cases")]
        public async Task DeleteCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var exampleCategory = _fixture.GetExampleCategory();
            repositoryMock.Setup(x => x.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);
            var input = new UseCase.DeleteCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.DeleteCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()
            ), Times.Once);
            repositoryMock.Verify(repository => repository.Delete(
                exampleCategory,
                It.IsAny<CancellationToken>()
            ), Times.Once);

            unitOfWorkMock.Verify(uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact(DisplayName = nameof(NotificationWhenCategoryNotFound))]
        [Trait("Application", "DeleteCategoryTest - Use Cases")]
        public async Task NotificationWhenCategoryNotFound()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var exampleGuid = Guid.NewGuid();
            repositoryMock.Setup(x => x.Get(
                    exampleGuid,
                    It.IsAny<CancellationToken>()
            )).ReturnsAsync(() => null!);
            var input = new UseCase.DeleteCategoryInput(exampleGuid);
            var useCase = new UseCase.DeleteCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Get(
            exampleGuid,
                It.IsAny<CancellationToken>()
            ), Times.Once);

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
