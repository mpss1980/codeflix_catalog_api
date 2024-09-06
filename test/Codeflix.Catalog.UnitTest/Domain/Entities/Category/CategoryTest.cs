using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Category;

[Collection(nameof(CategoryFixture))]
public class CategoryTest
{
    private readonly CategoryFixture _fixture;

    public CategoryTest(CategoryFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldCreateAValidaCategory))]
    public void ShouldCreateAValidaCategory()
    {
        var category = _fixture.GetCategory();
        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();
        category.Name.Should().NotBeNullOrEmpty();
        category.Description.Should().NotBeNullOrEmpty();
        category.IsActive.Should().BeTrue();
        category.CreatedAt.Should().BeBefore(DateTime.Now);
    }
    
}