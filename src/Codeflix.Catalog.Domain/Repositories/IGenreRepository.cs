using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Domain.Repositories;

public interface IGenreRepository : ISearchableRepository<Genre>
{
    public Task Insert(Genre genre, CancellationToken cancellationToken);
    public Task<Genre> Get(Guid id, CancellationToken cancellationToken);
    public Task Delete(Genre genre, CancellationToken cancellationToken);
    public Task Update(Genre genre, CancellationToken cancellationToken);
}