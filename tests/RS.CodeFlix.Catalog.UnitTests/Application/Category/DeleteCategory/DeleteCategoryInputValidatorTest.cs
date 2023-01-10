using FluentAssertions;
using RS.CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryInputValidatorTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryInputValidatorTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "DeleteCategoryInputValidationTest - Use Cases")]
        public void ValidationOk()
        {
            var validInput = new DeleteCategoryInput(Guid.NewGuid());
            validInput.Validate();

            validInput.Should().NotBeNull();
            validInput.Valid.Should().BeTrue();
            validInput.Notifications.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
        [Trait("Application", "DeleteCategoryInputValidationTest - Use Cases")]
        public void InvalidWhenEmptyGuidId()
        {
            var validInput = new DeleteCategoryInput(Guid.Empty);
            validInput.Validate();

            validInput.Should().NotBeNull();
            validInput.Valid.Should().BeFalse();
            validInput.Notifications.Should()
                .BeEquivalentTo(new List<Notification>() {
                new Notification("Id", "O Id é obrigatório"),
            });
            validInput.Notifications.ToList()[0].Message.Should().Be("O Id é obrigatório");
        }
    }
}
