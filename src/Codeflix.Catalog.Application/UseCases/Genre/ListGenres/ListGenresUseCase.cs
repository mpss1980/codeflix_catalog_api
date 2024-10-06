using Codeflix.Catalog.Domain.Repositories;
using DomainEntity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

public class ListGenresUseCase : IListGenresUseCase
{
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ListGenresUseCase(IGenreRepository genreRepository, ICategoryRepository categoryRepository)
    {
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ListGenresOutput> Handle(
        ListGenresInput request,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _genreRepository.Search(
            request.ToSearchInput(), cancellationToken
        );
        var output = ListGenresOutput.FromSearchOutput(searchOutput);

        var relatedCategoriesIds = searchOutput.Items
            .SelectMany(x => x.Categories)
            .Distinct()
            .ToList();

        if (relatedCategoriesIds.Count <= 0) return output;
        var categories =
            await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);
        output.FillWithCategoryNames(categories);

        return output;
    }
}