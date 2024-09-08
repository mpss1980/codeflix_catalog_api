using Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public interface IGetCategoryUseCase : IRequestHandler<GetCategoryInput, CategoryOutput>
{
    
}