using Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Category.GetCategory;

public class GetCategoryFixture : CategoryUseCaseBaseFixture
{
    
}

[CollectionDefinition(nameof(GetCategoryFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryFixture>
{
}