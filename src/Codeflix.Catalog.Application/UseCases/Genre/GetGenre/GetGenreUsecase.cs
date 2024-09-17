using Codeflix.Catalog.Application.UseCases.Genre.Common;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.UseCases.Genre.GetGenre;

public class GetGenreUsecase : IGetGenreUseCase
{
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetGenreUsecase(IGenreRepository genreRepository, ICategoryRepository categoryRepository)
    {
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreOutput> Handle(GetGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);
        var output = GenreOutput.FromGenre(genre);

        if (output.Categories.Count <= 0) return output;
        
        var categories = (await _categoryRepository.GetListByIds(
                output.Categories.Select(c => c.Id).ToList(), cancellationToken))
            .ToDictionary(x => x.Id);
        foreach (var category in output.Categories)
            category.Name = categories[category.Id].Name;

        return output;
    }
}