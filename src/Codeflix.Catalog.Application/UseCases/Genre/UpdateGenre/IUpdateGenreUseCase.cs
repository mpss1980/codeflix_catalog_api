using Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;

public interface IUpdateGenreUseCase : IRequestHandler<UpdateGenreInput, GenreOutput>
{
    
}