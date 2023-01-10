using FluentAssertions;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryInputValidatorTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
        [Trait("Application", "UpdateCategoryInputValidationTest - Use Cases")]
        public void DontValidateWhenEmptyGuid()
        {
            var input = _fixture.GetValidInput(Guid.Empty);
            input.Validate();

            input.Should().NotBeNull();
            input.Valid.Should().BeFalse();
            input.Notifications.Should().HaveCount(1);
            input.Notifications.Should()
                .BeEquivalentTo(new List<Notification>() {
                new Notification("Id", "O Id é obrigatório"),
            });
            input.Notifications.ToList()[0].Message.Should().Be("O Id é obrigatório");
        }

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "UpdateCategoryInputValidationTest - Use Cases")]
        public void ValidationOk()
        {
            var input = _fixture.GetValidInput();
            input.Validate();

            input.Valid.Should().BeTrue();
            input.Notifications.Should().HaveCount(0);
        }
    }
}
