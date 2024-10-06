using MediatR;

namespace Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

public interface IListGenresUseCase : IRequestHandler<ListGenresInput, ListGenresOutput>
{
    
}