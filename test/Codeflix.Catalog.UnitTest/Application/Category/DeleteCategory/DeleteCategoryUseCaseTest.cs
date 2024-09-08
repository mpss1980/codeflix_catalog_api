using Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using Codeflix.Catalog.UnitTest.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldDeleteACategory))]
    public async Task ShouldDeleteACategory()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var category = _fixture.GetCategory();

        repository.Setup(repo => repo.Get(
            category.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);

        var input = new DeleteCategoryInput(category.Id);
        var useCase = new DeleteCategoryUseCase(repository.Object, unitOfWork.Object);

        await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Get(
                category.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        repository.Verify(
            repo => repo.Delete(
                category,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWork.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ShouldThrowNotFoundExceptionWhenCategoryIsNotFound))]
    public async Task ShouldThrowNotFoundExceptionWhenCategoryIsNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var guid = Guid.NewGuid();

        repository.Setup(repo => repo.Get(
            guid,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Category '{guid}' not found.")
        );
        
        var input = new DeleteCategoryInput(guid);
        var useCase = new DeleteCategoryUseCase(repository.Object, unitOfWork.Object);
        
        var task = async () => await useCase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>();
        
        repository.Verify(
            repo => repo.Get(
                guid,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}