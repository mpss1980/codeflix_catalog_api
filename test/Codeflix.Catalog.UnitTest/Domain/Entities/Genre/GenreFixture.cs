using Bogus;
using Codeflix.Catalog.Domain.Commons;
using Codeflix.Catalog.UnitTest.Domain.Commons;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Domain.Entities.Genre;

public class GenreFixture : BaseFixture
{
    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public Entity.Genre GetGenre(
        bool isActive = true,
        List<Guid>? categories = null)
    {
        var genre = new Entity.Genre(GetValidName(), isActive);

        if (categories is not null)
            foreach (var categoryId in categories)
                genre.AddCategory(categoryId);
        return genre;
    }
}

[CollectionDefinition(nameof(GenreFixture))]
public class GenreFixtureCollection : ICollectionFixture<GenreFixture>
{
}