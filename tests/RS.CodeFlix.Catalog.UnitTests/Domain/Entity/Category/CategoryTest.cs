using Bogus;
using FluentAssertions;
using RS.CodeFlix.Catalog.Flunt.Notifications;
using Xunit;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
namespace RS.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;
   
    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
        //Assert.True(category.CreatedAt > dateTimeBefore);
        //Assert.True(category.CreatedAt < dateTimeAfter);
        //Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);       
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(" ")]
    [InlineData("")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(name!, validCategory.Description);
        
        category.Valid.Should().BeFalse();
        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should not be empty"),
                new Notification("Name", "Name should be at leats 3 characters long"),
           });
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(null!, validCategory.Description);
        //Action action = () => new DomainEntity.Category(name!, "Category Description");

        category.Valid.Should().BeFalse();
        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should not be null"),
           });
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, null!);

        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Description", "Description should not be null")
           });        
    }

    [Theory(DisplayName =nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    //[InlineData("a")]
    //[InlineData("1")]
    //[InlineData("ab")]
    //[InlineData("12")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var category = new DomainEntity.Category(invalidName, validCategory.Description);

        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should be at leats 3 characters long")
           });
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category(invalidName, validCategory.Description);
        
        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should be less or equal 255 characters long")
           });    
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category(validCategory.Name, invalidDescription);

        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Description", "Description should be less or equal 10.000 characters long")
           });
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);

        category.Activate();
        
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var categoryWothNewValues = _categoryTestFixture.GetValidCategory();

        category.Update(categoryWothNewValues.Name, categoryWothNewValues.Description);

        category.Name.Should().Be(categoryWothNewValues.Name);
        category.Description.Should().Be(categoryWothNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory();

        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
        
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData("  ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();

        category.Update(name!);

        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
               new Notification("Name", "Name should not be empty"),
               new Notification("Name", "Name should be at leats 3 characters long")
           });
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(null)]
    public void UpdateErrorWhenNameIsNull(string? name)
    {
        var category = _categoryTestFixture.GetValidCategory();

        category.Update(name!);

        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should not be null")
           });
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = _categoryTestFixture.GetValidCategory();
        category.Update(invalidName);

        category.Notifications.Should()
            .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should be at leats 3 characters long")
            }); 
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 2)
    {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfTests; i++)
        {
            //Se for impar, retorna 1 caractere, senão retorna 2 caracteres
            var isImpar = i % 2 == 1;
            //yield return new object[] { fixture.GetValidCategoryName().Substring(0, (isImpar ? 1 : 2)) };

            yield return new object[] { fixture.GetValidCategoryName()[..(isImpar ? 1 : 2)] };
        }
        //yield return new object[] { "1" };
        //yield return new object[] { "12" };
        //yield return new object[] { "a" };
        //yield return new object[] { "ca" };
        //yield return new object[] { "ux" };
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        //var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        category.Update(invalidName);

        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Name", "Name should be less or equal 255 characters long")
           });
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = _categoryTestFixture.GetValidCategory();
        //var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        category.Update(category.Name, invalidDescription);
        category.Notifications.Should()
           .BeEquivalentTo(new List<Notification>() {
                new Notification("Description", "Description should be less or equal 10.000 characters long")
           });
    }
}

