using Codeflix.Catalog.UnitTest.Domain.Commons;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Category;

public class CategoryFixture : BaseFixture
{
    public CategoryFixture() : base()
    {
    }

    public Entity.Category GetCategory(bool? isActive = null)
    {
        return isActive != null
            ? new Entity.Category(GetValidName(), GetValidName(), isActive.Value)
            : new Entity.Category(GetValidName(), GetValidDescription());
    }

    private string GetValidName() => Faker.Random.String2(3, 255);

    private string GetValidDescription() => Faker.Random.String2(1, 10000);
}

[CollectionDefinition(nameof(CategoryFixture))]
public class CategoryFixtureCollection : ICollectionFixture<CategoryFixture>
{
}