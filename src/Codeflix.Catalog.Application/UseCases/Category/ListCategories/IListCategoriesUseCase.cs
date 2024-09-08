using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.ListCategories;

public interface IListCategoriesUseCase : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
    
}