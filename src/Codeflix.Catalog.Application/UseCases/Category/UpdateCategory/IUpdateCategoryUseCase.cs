using Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

public interface IUpdateCategoryUseCase : IRequestHandler<UpdateCategoryInput, CategoryOutput>
{
    
}