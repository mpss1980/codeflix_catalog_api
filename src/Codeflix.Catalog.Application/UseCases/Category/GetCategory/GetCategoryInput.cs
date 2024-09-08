using Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryInput : IRequest<CategoryOutput>
{
    public Guid Id { get; set; }
    public GetCategoryInput(Guid id)
     => Id = id;
}