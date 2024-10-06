using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Genre.Common;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;

public class UpdateGenreUseCase : IUpdateGenreUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenreUseCase(
        IGenreRepository genreRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork
    )
    {
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenreOutput> Handle(UpdateGenreInput request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);
        genre.Update(request.Name);

        if (request.IsActive is not null && request.IsActive != genre.IsActive)
        {
            UpdateGenreActivity(genre, request.IsActive.Value);
        }

        if (request.CategoriesIds is not null)
        {
            genre.RemoveAllCategories();
            if (request.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(request, cancellationToken);
                request.CategoriesIds?.ForEach(genre.AddCategory);
            }
        }

        await _genreRepository.Update(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return GenreOutput.FromGenre(genre);
    }

    private async Task ValidateCategoriesIds(
        UpdateGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var idsInPersistence = await _categoryRepository.GetIdListByIds(
            request.CategoriesIds!,
            cancellationToken
        );
        
        if (idsInPersistence.Count >= request.CategoriesIds!.Count)
        {
            return;
        }

        var notFoundIds = request.CategoriesIds.FindAll(x => !idsInPersistence.Contains(x));
        var notFoundIdAsString = string.Join(", ", notFoundIds);
        throw new RelatedAggregateException(
            $"Related category id (or ids) not found: {notFoundIdAsString}");
    }

    private static void UpdateGenreActivity(Domain.Entities.Genre genre, bool isActive)
    {
        if (isActive)
        {
            genre.Activate();
            return;
        }

        genre.Deactivate();
    }
}