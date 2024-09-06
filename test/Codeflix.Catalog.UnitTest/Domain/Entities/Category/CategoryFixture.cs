using Codeflix.Catalog.UnitTest.Domain.Commons;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Category;

public class CategoryFixture : BaseFixture
{
    public CategoryFixture() : base()
    {
    }

    public Entity.Category GetCategory()
    {
        return new Entity.Category(Faker.Random.Word(), Faker.Lorem.Paragraph());
    }
}

[CollectionDefinition(nameof(CategoryFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
{
}