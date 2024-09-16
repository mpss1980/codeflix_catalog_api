using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Genre.Common;
using Codeflix.Catalog.Domain.Repositories;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.UseCases.Genre.CreateCategory;

public class CreateGenreUseCase : ICreateGenreUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenreUseCase(IGenreRepository genreRepository, ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _genreRepository = genreRepository;
    }

    public async Task<GenreOutput> Handle(CreateGenreInput request, CancellationToken cancellationToken)
    {
        var genre = new Entity.Genre(
            request.Name,
            request.IsActive
        );

        if ((request.CategoriesIds?.Count ?? 0) > 0)
        {
            await ValidateCategoriesIds(request.CategoriesIds!, cancellationToken);
            request.CategoriesIds?.ForEach(genre.AddCategory);
        }

        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GenreOutput.FromGenre(genre);
    }
    
    private async Task ValidateCategoriesIds(List<Guid> categoriesIds, CancellationToken cancellationToken)
    {
        var idsInPersistence = await _categoryRepository.GetIdListByIds(
            categoriesIds,
            cancellationToken
        );
        if (idsInPersistence.Count() < categoriesIds.Count)
        {
            var notFoundIds = categoriesIds.FindAll(x => !idsInPersistence.Contains(x));
            var notFoundIdsAsString = String.Join(", ", notFoundIds);
            throw new RelatedAggregateException(
                $"Related category ud (or dis) not found: {notFoundIdsAsString}");
        }
    }
}