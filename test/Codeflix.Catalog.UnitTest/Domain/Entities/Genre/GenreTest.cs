using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Genre;

[Collection(nameof(GenreFixture))]
public class GenreTest
{
    private readonly GenreFixture _fixture;

    public GenreTest(GenreFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldCreateAValidGenreOnlyWithName))]
    public void ShouldCreateAValidGenreOnlyWithName()
    {
        var genreName = _fixture.GetValidName();
        var genre = new Entity.Genre(genreName);

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().BeTrue();
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
    }

    [Theory(DisplayName = nameof(ShouldCreateAValidGenreWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldCreateAValidGenreWithIsActive(bool isActive)
    {
        var genreName = _fixture.GetValidName();
        var genre = new Entity.Genre(genreName, isActive);

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(isActive);
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
    }

    [Theory(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenNameIsNullOrEmpty))]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowEntityValidationExceptionWhenNameIsNullOrEmpty(string? name)
    {
        var action = () => new Entity.Genre(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty");
    }

    [Fact(DisplayName = nameof(ShouldActivateGenre))]
    public void ShouldActivateGenre()
    {
        var genreName = _fixture.GetValidName();
        var genre = new Entity.Genre(genreName, false);

        genre.Activate();

        genre.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(ShouldDeactivateGenre))]
    public void ShouldDeactivateGenre()
    {
        var genreName = _fixture.GetValidName();
        var genre = new Entity.Genre(genreName, true);

        genre.Deactivate();

        genre.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ShouldUpdateGenre))]
    public void ShouldUpdateGenre()
    {
        var genre = _fixture.GetGenre();
        var newName = _fixture.Faker.Random.String2(3, 255);
        var oldIsActive = genre.IsActive;

        genre.Update(newName);

        genre.Should().NotBeNull();
        genre.Id.Should().NotBeEmpty();
        genre.Name.Should().Be(newName);
        genre.IsActive.Should().Be(oldIsActive);
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
    }

    [Theory(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenUpdateNameNullOrEmpty))]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ShouldThrowEntityValidationExceptionWhenUpdateNameNullOrEmpty(string? name)
    {
        var validGenre = _fixture.GetGenre();

        var action = () => validGenre.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty");
    }

    [Fact(DisplayName = nameof(ShouldAddACategory))]
    public void ShouldAddACategory()
    {
        var genre = _fixture.GetGenre();
        var categoryId = Guid.NewGuid();

        genre.AddCategory(categoryId);

        genre.Categories.Should().HaveCount(1);
        genre.Categories.Should().Contain(categoryId);
    }

    [Fact(DisplayName = nameof(ShouldRemoveACategory))]
    public void ShouldRemoveACategory()
    {
        var categoryId = Guid.NewGuid();
        var genre = _fixture.GetGenre(
            categories: new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                categoryId,
                Guid.NewGuid(),
                Guid.NewGuid(),
            });

        genre.RemoveCategory(categoryId);

        genre.Categories.Should().HaveCount(4);
        genre.Categories.Should().NotContain(categoryId);
    }
    
    [Fact(DisplayName = nameof(ShouldRemoveAllCategories))]
    public void ShouldRemoveAllCategories()
    {
        var genre = _fixture.GetGenre(
            categories: new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            });

        genre.RemoveAllCategories();

        genre.Categories.Should().HaveCount(0);
    }
}