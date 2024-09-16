using Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Genre.CreateCategory;

public interface ICreateGenreUseCase : IRequestHandler<CreateGenreInput, GenreOutput>
{
    
}