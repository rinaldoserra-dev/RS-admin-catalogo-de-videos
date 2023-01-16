using FluentAssertions;
using Moq;
using RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;
using UseCases = RS.CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture createCategoryTestFixture)
        {
            _fixture = createCategoryTestFixture;
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategoryTest - Use Cases")]
        public async Task CreateCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var input = _fixture.GetInput();

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<DomainEntity.Category>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);

            unitOfWorkMock.Verify(
               uow => uow.Commit(It.IsAny<CancellationToken>()),
               Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(NofificationWhenCantInstantiateCategory))]
        [Trait("Application", "CreateCategoryTest - Use Cases")]
        [MemberData(
            nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
            parameters: 24,
            MemberType = typeof(CreateCategoryTestDataGenerator)
        )]
        public async void NofificationWhenCantInstantiateCategory(
            CreateCategoryInput input,
            string PropertyMessage,
            string NotificationMessage
        )
        {
            var useCase = new UseCases.CreateCategory(
                _fixture.GetRepositoryMock().Object,
                _fixture.GetUnitOfWorkMock().Object
            );

            await useCase.Handle(input, CancellationToken.None);

            useCase.Valid.Should().BeFalse();
            useCase.Notifications.Should()
               .BeEquivalentTo(new List<Notification>() {
                new Notification(PropertyMessage, NotificationMessage)
               });
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Application", "CreateCategoryTest - Use Cases")]
        public async void CreateCategoryWithOnlyName()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<DomainEntity.Category>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);

            unitOfWorkMock.Verify(
               uow => uow.Commit(It.IsAny<CancellationToken>()),
               Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Application", "CreateCategoryTest - Use Cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<DomainEntity.Category>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once);

            unitOfWorkMock.Verify(
               uow => uow.Commit(It.IsAny<CancellationToken>()),
               Times.Once
            );

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }
    }
}
