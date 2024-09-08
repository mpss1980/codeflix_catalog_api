using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Repositories;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

public class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryUseCase(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _repository.Get(request.Id, cancellationToken);
        category.Update(request.Name, request.Description);
        if (request.IsActive != null && request.IsActive != category.IsActive)
        {
            category = UpdateCategoryIsActive(category, (bool) request.IsActive);
        }

        await _repository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return CategoryOutput.FromCategory(category);
    }

    private Entity.Category UpdateCategoryIsActive(Entity.Category category, bool isActive)
    {
        if (isActive)
        {
            category.Activate();
            return category;
        }

        category.Deactivate();
        return category;
    }
}