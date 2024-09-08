using Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Category.DeleteCategory;

public class DeleteCategoryFixture : CategoryUseCaseBaseFixture
{
    
}

[CollectionDefinition(nameof(DeleteCategoryFixture))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryFixture>
{
}