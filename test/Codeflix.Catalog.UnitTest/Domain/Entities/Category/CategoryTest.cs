using Bogus;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Category;

[Collection(nameof(CategoryFixture))]
public class CategoryTest
{
    private readonly CategoryFixture _fixture;

    public CategoryTest(CategoryFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldCreateAValidCategory))]
    public void ShouldCreateAValidCategory()
    {
        var category = _fixture.GetCategory();

        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();
        category.Name.Should().NotBeNullOrEmpty();
        category.Description.Should().NotBeNullOrEmpty();
        category.IsActive.Should().BeTrue();
        category.CreatedAt.Should().BeBefore(DateTime.Now);
    }

    [Theory(DisplayName = nameof(ShouldCreateAValidCategoryWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldCreateAValidCategoryWithIsActive(bool isActive)
    {
        var category = _fixture.GetCategory(isActive);

        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();
        category.Name.Should().NotBeNullOrEmpty();
        category.Description.Should().NotBeNullOrEmpty();
        category.IsActive.Should().Be(isActive);
        category.CreatedAt.Should().BeBefore(DateTime.Now);
    }

    [Theory(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenNameIsNullOrEmpty))]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowEntityValidationExceptionWhenNameIsNullOrEmpty(string? name)
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            new Entity.Category(name!, validCategory.Description);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty");
    }

    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenNameIsLessThan3CharactersLong))]
    public void ShouldThrowEntityValidationExceptionWhenNameIsLessThan3CharactersLong()
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            new Entity.Category("ab", validCategory.Description);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters long");
    }

    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenNameIsGreaterThan255CharactersLong))]
    public void ShouldThrowEntityValidationExceptionWhenNameIsGreaterThan255CharactersLong()
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            new Entity.Category(_fixture.Faker.Random.String2(256), validCategory.Description);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal to 255 characters long");
    }

    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenDescriptionIsNull))]
    public void ShouldThrowEntityValidationExceptionWhenDescriptionIsNull()
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            new Entity.Category(validCategory.Name, null!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenDescriptionIsGreaterThan10000CharactersLong))]
    public void ShouldThrowEntityValidationExceptionWhenDescriptionIsGreaterThan10000CharactersLong()
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            new Entity.Category(validCategory.Name, _fixture.Faker.Random.String2(10001));

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal to 10000 characters long");
    }

    [Fact(DisplayName = nameof(ShouldActivateCategory))]
    public void ShouldActivateCategory()
    {
        var category = _fixture.GetCategory(false);

        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(ShouldDeactivateCategory))]
    public void ShouldDeactivateCategory()
    {
        var category = _fixture.GetCategory();

        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ShouldUpdateCategory))]
    public void ShouldUpdateCategory()
    {
        var category = _fixture.GetCategory();

        var newName = _fixture.Faker.Random.String2(3, 255);
        var newDescription = _fixture.Faker.Random.String2(1, 10000);

        category.Update(newName, newDescription);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(newDescription);
    }

    [Fact(DisplayName = nameof(ShouldUpdateOnlyName))]
    public void ShouldUpdateOnlyName()
    {
        var category = _fixture.GetCategory();

        var newName = _fixture.Faker.Random.String2(3, 255);

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(category.Description);
    }

    [Theory(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenUpdateNameNullOrEmpty))]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowEntityValidationExceptionWhenUpdateNameNullOrEmpty(string? name)
    {
        var validCategory = _fixture.GetCategory();

        var action = () => validCategory.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty");
    }

    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenUpdateNameWithLessThan3CharactersLong))]
    public void ShouldThrowEntityValidationExceptionWhenUpdateNameWithLessThan3CharactersLong()
    {
        var validCategory = _fixture.GetCategory();

        var action = () => validCategory.Update("av");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should have at least 3 characters long");
    }

    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenUpdateNameGreaterThan255CharactersLong))]
    public void ShouldThrowEntityValidationExceptionWhenUpdateNameGreaterThan255CharactersLong()
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            validCategory.Update(_fixture.Faker.Random.String2(256));

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal to 255 characters long");
    }
    
    [Fact(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenUpdateDescriptionIsGreaterThan10000CharactersLong))]
    public void ShouldThrowEntityValidationExceptionWhenUpdateDescriptionIsGreaterThan10000CharactersLong()
    {
        var validCategory = _fixture.GetCategory();

        var action = () =>
            validCategory.Update(validCategory.Name, _fixture.Faker.Random.String2(10001));

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal to 10000 characters long");
    }
}