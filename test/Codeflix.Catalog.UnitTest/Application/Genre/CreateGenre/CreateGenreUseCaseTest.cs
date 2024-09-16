using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.UnitTest.Application.Category.CreateCategory;
using FluentAssertions;
using Moq;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Genre.CreateGenre;

[Collection(nameof(CreateGenreFixture))]
public class CreateGenreUseCaseTest
{
    private readonly CreateGenreFixture _fixture;

    public CreateGenreUseCaseTest(CreateGenreFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldCreateGenre))]
    public async void ShouldCreateGenre()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();

        var useCase = new CreateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = _fixture.GetCreateGenreInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Insert(
                It.IsAny<Entity.Genre>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWork.Verify(
            uow => uow.Commit(CancellationToken.None),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        output.Categories.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ShouldCreateGenreWithRelatedCategories))]
    public async void ShouldCreateGenreWithRelatedCategories()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();

        var input = _fixture.GetCreateGenreInputWithCategories();
        categoryRepository.Setup(
            x => x.GetIdListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync((IReadOnlyList<Guid>)input.CategoriesIds!);

        var useCase = new CreateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Insert(
                It.IsAny<Entity.Genre>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWork.Verify(
            uow => uow.Commit(CancellationToken.None),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        output.Categories.Should().HaveCount(input.CategoriesIds?.Count ?? 0);
        input.CategoriesIds?.ForEach(
            categoryId => output.Categories.Should()
                .Contain(relation => relation.Id == categoryId)
        );
    }
    
    [Fact(DisplayName = nameof(ShouldThrowRelatedExceptionWhenRelatedCategoriesIsNotFound))]
    public async void ShouldThrowRelatedExceptionWhenRelatedCategoriesIsNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();

        var input = _fixture.GetCreateGenreInputWithCategories();
        var lastCategoryId = input.CategoriesIds![^1];
        
        categoryRepository.Setup(
            x => x.GetIdListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync((IReadOnlyList<Guid>)input.CategoriesIds!
            .FindAll(x => x != lastCategoryId));

        var useCase = new CreateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var action = async () => await useCase.Handle(input, CancellationToken.None);
        
        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related category id (or dis) not found: {lastCategoryId}");

       categoryRepository.Verify(
            x => x.GetIdListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
    
    [Theory(DisplayName = nameof(ShouldThrowEntityValidationExceptionWhenNameIsInvalid))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public async void ShouldThrowEntityValidationExceptionWhenNameIsInvalid(string name)
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();

        var useCase = new CreateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = _fixture.GetCreateGenreInput();
        input.Name = name;

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage($"Name should not be null or empty");
    }
}