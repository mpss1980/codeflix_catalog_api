using Codeflix.Catalog.UnitTest.Application.Genre.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Genre.DeleteGenre;

public class DeleteGenreFixture : GenreUseCaseBaseFixture
{
    
}

[CollectionDefinition(nameof(DeleteGenreFixture))]
public class DeleteGenreFixtureCollection : ICollectionFixture<DeleteGenreFixture>
{
    
}