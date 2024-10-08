using Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;

public interface ICreateGenreUseCase : IRequestHandler<CreateGenreInput, GenreOutput>
{
    
}