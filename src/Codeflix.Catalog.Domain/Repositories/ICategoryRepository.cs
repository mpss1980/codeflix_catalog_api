using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Domain.Repositories;

public interface ICategoryRepository : ISearchableRepository<Category>
{
    public Task Insert(Category category, CancellationToken cancellationToken);
    public Task<Category> Get(Guid id, CancellationToken cancellationToken);
    public Task Delete(Category category, CancellationToken cancellationToken);
    public Task Update(Category category, CancellationToken cancellationToken);
    public Task<IReadOnlyList<Guid>> GetIdListByIds(List<Guid> ids, CancellationToken cancellationToken);
    public Task<IReadOnlyList<Category>> GetListByIds(List<Guid> ids, CancellationToken cancellationToken);
}