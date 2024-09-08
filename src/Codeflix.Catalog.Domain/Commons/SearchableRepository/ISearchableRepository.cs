namespace Codeflix.Catalog.Domain.Commons.SearchableRepository;

public interface ISearchableRepository<T> where T : Aggregate
{
    Task<SearchOutput<T>> Search(SearchInput input, CancellationToken cancellationToken);
}