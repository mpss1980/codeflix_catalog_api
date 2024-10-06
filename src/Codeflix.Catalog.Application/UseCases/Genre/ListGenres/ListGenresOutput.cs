using Codeflix.Catalog.Application.Common;
using Codeflix.Catalog.Application.UseCases.Genre.Common;
using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using DomainEntity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

public class ListGenresOutput : PaginatedListOutput<GenreOutput>
{
    public ListGenresOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<GenreOutput> items
    ) : base(page, perPage, total, items)
    {
    }

    public static ListGenresOutput FromSearchOutput(SearchOutput<DomainEntity.Genre> searchOutput)
        => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items.Select(GenreOutput.FromGenre).ToList()
        );

    internal void FillWithCategoryNames(IReadOnlyList<DomainEntity.Category> categories)
    {
        foreach (var item in Items)
            foreach (var categoryOutput in item.Categories)
                categoryOutput.Name = categories?.FirstOrDefault(x => x.Id == categoryOutput.Id)?.Name;
    }
}