using Codeflix.Catalog.Application.UseCases.Genre.ListGenres;
using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using Codeflix.Catalog.UnitTest.Application.Genre.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Genre.ListGenres;

public class ListGenresFixture : GenreUseCaseBaseFixture
{
    public ListGenresInput GetListGenreInput()
    {
        var random = new Random();
        return new ListGenresInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}

[CollectionDefinition(nameof(ListGenresFixture))]
public class ListGenresFixtureCollection : ICollectionFixture<ListGenresFixture>
{
    
}