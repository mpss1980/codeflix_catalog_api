using Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Genre.CreateCategory;

public class CreateGenreInput : IRequest<GenreOutput>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }
    
    public CreateGenreInput(string name, bool isActive = true, List<Guid>? categoriesIds = null)
    {
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }
}