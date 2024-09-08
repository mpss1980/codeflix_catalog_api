using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Repositories;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryOutput> Handle(CreateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = new Entity.Category(
            request.Name,
            request.Description,
            request.IsActive
        );
        
        await _categoryRepository.Insert(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        
        return CategoryOutput.FromCategory(category);
    }
}