using Codeflix.Catalog.Application.Common;
using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.ListCategories;

public class ListCategoriesInput : PaginatedListInput, IRequest<ListCategoriesOutput>
{
    public ListCategoriesInput(int page = 1, int perPage = 15, string search = "", string sort = "",
        SearchOrder dir = SearchOrder.Asc) : base(page, perPage, search, sort, dir)
    {
    }
}