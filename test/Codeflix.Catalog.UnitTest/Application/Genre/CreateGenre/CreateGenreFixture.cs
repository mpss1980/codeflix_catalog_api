using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Application.UseCases.Genre.CreateCategory;
using Codeflix.Catalog.UnitTest.Application.Category.Common;
using Codeflix.Catalog.UnitTest.Application.Category.CreateCategory;
using Codeflix.Catalog.UnitTest.Application.Genre.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Genre.CreateGenre;

public class CreateGenreFixture : GenreUseCaseBaseFixture
{
    public CreateGenreInput GetCreateGenreInput()
        => new(
            GetValidName(),
            Faker.Random.Bool()
        );

    public CreateGenreInput GetCreateGenreInputWithCategories()
    {
        var categoriesIds = Enumerable.Range(1, Faker.Random.Int(min: 1, max: 10))
            .Select(_ => Guid.NewGuid()).ToList();
        return new CreateGenreInput(GetValidName(), Faker.Random.Bool(), categoriesIds);
    }
}

[CollectionDefinition(nameof(CreateGenreFixture))]
public class CreateGenreFixtureCollection : ICollectionFixture<CreateGenreFixture>
{
}