using Codeflix.Catalog.UnitTest.Application.Genre.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Genre.GetGenre;

public class GetGenreFixture : GenreUseCaseBaseFixture
{
    
}

[CollectionDefinition(nameof(GetGenreFixture))]
public class GetGenreFixtureCollection : ICollectionFixture<GetGenreFixture>
{
    
}