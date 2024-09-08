using Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategoryUseCase : IRequestHandler<CreateCategoryInput, CategoryOutput>
{
    
}