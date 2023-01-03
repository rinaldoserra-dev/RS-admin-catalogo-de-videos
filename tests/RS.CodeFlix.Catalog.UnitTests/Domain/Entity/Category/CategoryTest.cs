using FluentAssertions;
using RS.CodeFlix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = RS.CodeFlix.Catalog.Domain.Entity;
namespace RS.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };
        var dateTimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
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
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };
        var dateTimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddDays(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);       
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category(name!, "Category Description");
        //Action action = () => new DomainEntity.Category(name!, "Category Description");

        Assert.Equal("Name should not be empty or null", category.Notifications.Where(n => n.Property == "Name_IsNullOrWhiteSpace").FirstOrDefault()!.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {

        var category = new DomainEntity.Category("Category Name", null!);

        Assert.Equal("Description should not be null", category.Notifications.Where(n => n.Property == "Description_IsNotNull").FirstOrDefault()!.Message);
    }

    [Theory(DisplayName =nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData("ab")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category(invalidName, "Category Ok Description");

        Assert.Equal("Name should be at leats 3 characters long", category.Notifications.Where(n => n.Property == "Name_HasMinLen").FirstOrDefault()!.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category(invalidName, "Category Ok Description");

        Assert.Equal("Name should be less or equal 255 characters long", category.Notifications.Where(n => n.Property == "Name_HasMaxLen").FirstOrDefault()!.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category("Category Name", invalidDescription);

        Assert.Equal("Description should be less or equal 10.000 characters long", category.Notifications.Where(n => n.Property == "Description_HasMaxLen").FirstOrDefault()!.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };
       
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);

        category.Activate();
        
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");

        var newValues = new
        {
            Name = "New Name",
            Description = "New Description",
        };

        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");

        var newValues = new { Name = "New Name" };
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
        
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");

        category.Update(name!);

        Assert.Equal("Name should not be empty or null", category.Notifications.Where(n => n.Property == "Name_IsNullOrWhiteSpace").FirstOrDefault()!.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData("ab")]
    [InlineData("12")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        category.Update(invalidName);

        Assert.Equal("Name should be at leats 3 characters long", category.Notifications.Where(n => n.Property == "Name_HasMinLen").FirstOrDefault()!.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        category.Update(invalidName);

        Assert.Equal("Name should be less or equal 255 characters long", category.Notifications.Where(n => n.Property == "Name_HasMaxLen").FirstOrDefault()!.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        category.Update(category.Name, invalidDescription);

        Assert.Equal("Description should be less or equal 10.000 characters long", category.Notifications.Where(n => n.Property == "Description_HasMaxLen").FirstOrDefault()!.Message);
    }
}

