using Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Category.ListCategories;

public class ListCategoriesFixture : CategoryUseCaseBaseFixture
{
    public List<Entity.Category> GetCategories(int qtd = 10)
    {
        var categories = new List<Entity.Category>();
        for (var i = 0; i < qtd; i++)
        {
            var category = GetCategory();
            categories.Add(category);
        }

        return categories;
    }

    public ListCategoriesInput GetListCategoriesInput()
    {
        var random = new Random();
        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}

[CollectionDefinition(nameof(ListCategoriesFixture))]
public class ListCategoriesFixtureCollection : ICollectionFixture<ListCategoriesFixture>
{
}