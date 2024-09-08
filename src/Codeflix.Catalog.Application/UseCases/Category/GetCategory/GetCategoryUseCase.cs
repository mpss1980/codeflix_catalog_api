using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Repositories;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;

public class GetCategoryUseCase : IGetCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryUseCase(ICategoryRepository categoryRepository)
     => _categoryRepository = categoryRepository;

    public async Task<CategoryOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        return CategoryOutput.FromCategory(category);
    }
}