using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Application.UseCases.Genre.CreateCategory;
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

    // [Fact(DisplayName = nameof(ShouldCreateCategoryOnlyWithNameParam))]
    // public async void ShouldCreateCategoryOnlyWithNameParam()
    // {
    //     var repository = _fixture.GetRepositoryMock();
    //     var unitOfWork = _fixture.GetUnitOfWorkMock();
    //     var useCase = new CreateCategoryUseCase(repository.Object, unitOfWork.Object);
    //     var input = new CreateCategoryInput(_fixture.GetValidName());
    //
    //     var output = await useCase.Handle(input, CancellationToken.None);
    //
    //     repository.Verify(
    //         repo => repo.Insert(
    //             It.IsAny<Entity.Category>(),
    //             It.IsAny<CancellationToken>()
    //         ),
    //         Times.Once
    //     );
    //
    //     unitOfWork.Verify(
    //         uow => uow.Commit(CancellationToken.None),
    //         Times.Once
    //     );
    //
    //     output.Should().NotBeNull();
    //     output.Id.Should().NotBeEmpty();
    //     output.Name.Should().Be(input.Name);
    //     output.Description.Should().Be("");
    //     output.IsActive.Should().BeTrue();
    //     output.CreatedAt.Should().NotBeSameDateAs(default);
    // }
    //
    // [Fact(DisplayName = nameof(ShouldCreateCategoryOnlyWithNameAndDescriptionParam))]
    // public async void ShouldCreateCategoryOnlyWithNameAndDescriptionParam()
    // {
    //     var repository = _fixture.GetRepositoryMock();
    //     var unitOfWork = _fixture.GetUnitOfWorkMock();
    //     var useCase = new CreateCategoryUseCase(repository.Object, unitOfWork.Object);
    //     var input = new CreateCategoryInput(
    //         _fixture.GetValidName(),
    //         _fixture.GetValidDescription()
    //     );
    //
    //     var output = await useCase.Handle(input, CancellationToken.None);
    //
    //     repository.Verify(
    //         repo => repo.Insert(
    //             It.IsAny<Entity.Category>(),
    //             It.IsAny<CancellationToken>()
    //         ),
    //         Times.Once
    //     );
    //
    //     unitOfWork.Verify(
    //         uow => uow.Commit(CancellationToken.None),
    //         Times.Once
    //     );
    //
    //     output.Should().NotBeNull();
    //     output.Id.Should().NotBeEmpty();
    //     output.Name.Should().Be(input.Name);
    //     output.Description.Should().Be(input.Description);
    //     output.IsActive.Should().BeTrue();
    //     output.CreatedAt.Should().NotBeSameDateAs(default);
    // }
    //
    // [Theory(DisplayName = nameof(ShouldThrowExceptionWhenCannotCreateCategory))]
    // [MemberData(
    //     nameof(CreateCategoryDataGenerator.GetInvalidInputs),
    //     MemberType = typeof(CreateCategoryDataGenerator)
    // )]
    // public async void ShouldThrowExceptionWhenCannotCreateCategory(
    //     CreateCategoryInput input,
    //     string exceptionMessage
    // )
    // {
    //     var useCase = new CreateCategoryUseCase(
    //         _fixture.GetRepositoryMock().Object,
    //         _fixture.GetUnitOfWorkMock().Object
    //     );
    //     
    //     Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);
    //     
    //     await task.Should().ThrowAsync<EntityValidationException>()
    //         .WithMessage(exceptionMessage);
    // }
}