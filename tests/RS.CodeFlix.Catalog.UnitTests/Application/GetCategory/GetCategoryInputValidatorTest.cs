using FluentAssertions;
using RS.CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;

namespace RS.CodeFlix.Catalog.UnitTests.Application.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryInputValidatorTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "GetCategoryInputValidationTest - Use Cases")]
        public void ValidationOk()
        {
            var validInput = new GetCategoryInput(Guid.NewGuid());
            validInput.Validate();

            validInput.Should().NotBeNull();
            validInput.Valid.Should().BeTrue();
            validInput.Notifications.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
        [Trait("Application", "GetCategoryInputValidationTest - Use Cases")]
        public void InvalidWhenEmptyGuidId()
        {
            var validInput = new GetCategoryInput(Guid.Empty);
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
