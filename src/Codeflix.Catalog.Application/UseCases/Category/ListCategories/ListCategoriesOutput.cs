using Codeflix.Catalog.Application.Common;
using Codeflix.Catalog.Application.UseCases.Category.Common;

namespace Codeflix.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategoriesOutput : PaginatedListOutput<CategoryOutput>
{
    public ListCategoriesOutput(int page, int perPage, int total, IReadOnlyList<CategoryOutput> items)
        : base(page, perPage, total, items)
    {
    }
}