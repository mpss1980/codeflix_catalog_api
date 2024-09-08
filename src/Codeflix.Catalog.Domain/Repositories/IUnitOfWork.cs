namespace Codeflix.Catalog.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit(CancellationToken cancellationToken);
}