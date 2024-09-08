using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.UnitTest.Application.Category.CreateCategory;
using Codeflix.Catalog.UnitTest.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixture))]
public class UpdateCategoryUseCaseTest
{
    private readonly UpdateCategoryFixture _fixture;

    public UpdateCategoryUseCaseTest(UpdateCategoryFixture fixture)
        => _fixture = fixture;

    [Theory(DisplayName = nameof(ShouldUpdateCategory))]
    [MemberData(
        nameof(UpdateCategoriesDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoriesDataGenerator)
    )]
    public async Task ShouldUpdateCategory(
        Entity.Category category,
        UpdateCategoryInput input
    )
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        repository.Setup(repo => repo.Get(
            category.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);

        var useCase = new UpdateCategoryUseCase(repository.Object, unitOfWork.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);

        repository.Verify(
            repo => repo.Get(
                category.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        repository.Verify(
            repo => repo.Update(
                category,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWork.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ShouldUpdateCategoryWithoutProvidingIsActive))]
    [MemberData(
        nameof(UpdateCategoriesDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoriesDataGenerator)
    )]
    public async Task ShouldUpdateCategoryWithoutProvidingIsActive(
        Entity.Category category,
        UpdateCategoryInput input
    )
    {
        var inputToUpdate = new UpdateCategoryInput(
            input.Id,
            input.Name,
            input.Description
        );

        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        repository.Setup(repo => repo.Get(
            category.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);

        var useCase = new UpdateCategoryUseCase(repository.Object, unitOfWork.Object);
        var output = await useCase.Handle(inputToUpdate, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(inputToUpdate.Name);
        output.Description.Should().Be(inputToUpdate.Description);
        output.IsActive.Should().Be(category.IsActive);

        repository.Verify(
            repo => repo.Get(
                category.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        repository.Verify(
            repo => repo.Update(
                category,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWork.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ShouldUpdateCategoryOnlyPassingName))]
    [MemberData(
        nameof(UpdateCategoriesDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoriesDataGenerator)
    )]
    public async Task ShouldUpdateCategoryOnlyPassingName(
        Entity.Category category,
        UpdateCategoryInput input
    )
    {
        var inputToUpdate = new UpdateCategoryInput(
            input.Id,
            input.Name
        );

        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        repository.Setup(repo => repo.Get(
            category.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);

        var useCase = new UpdateCategoryUseCase(repository.Object, unitOfWork.Object);
        var output = await useCase.Handle(inputToUpdate, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(inputToUpdate.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);

        repository.Verify(
            repo => repo.Get(
                category.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        repository.Verify(
            repo => repo.Update(
                category,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWork.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ShouldThrowExceptionWhenCategoryNotFound))]
    public async Task ShouldThrowExceptionWhenCategoryNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();

        repository.Setup(repo => repo.Get(
            input.Id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Category '{input.Id}' not found")
        );

        var useCase = new UpdateCategoryUseCase(repository.Object, unitOfWork.Object);
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repository.Verify(
            repo => repo.Get(
                input.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Theory(DisplayName = nameof(ShouldThrowExceptionWhenCannotUpdateCategory))]
    [MemberData(
        nameof(UpdateCategoriesDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoriesDataGenerator)
    )]
    public async void ShouldThrowExceptionWhenCannotUpdateCategory(
        UpdateCategoryInput input,
        string exceptionMessage
    )
    {
        var category = _fixture.GetCategory();
        input.Id = category.Id;

        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        repository.Setup(repo => repo.Get(
            category.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);
        
        var useCase = new UpdateCategoryUseCase(repository.Object, unitOfWork.Object);
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
        
        repository.Verify(
            repo => repo.Get(
                category.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}