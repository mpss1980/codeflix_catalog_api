using Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace Codeflix.Catalog.UnitTest.Application.Category.ListCategories;

public class ListCategoriesDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameters()
    {
        var fixture = new ListCategoriesFixture();
        var input = fixture.GetListCategoriesInput();
        var inputsList = new List<object[]>
        {
            new object[]
            {
                new ListCategoriesInput()
            },
            new object[]
            {
                new ListCategoriesInput(input.Page)
            },
            new object[]
            {
                new ListCategoriesInput(
                    input.Page,
                    input.PerPage
                )
            },
            new object[]
            {
                new ListCategoriesInput(
                    input.Page,
                    input.PerPage,
                    input.Search
                )
            },
            new object[]
            {
                new ListCategoriesInput(
                    input.Page,
                    input.PerPage,
                    input.Search,
                    input.Sort
                )
            },
            new object[]
            {
                input
            }
        };
        return inputsList;
    }
}