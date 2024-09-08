using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Category.CreateCategory;

[Collection(nameof(CreateCategoryFixture))]
public class CreateCategoryUseCaseTest
{
    private readonly CreateCategoryFixture _fixture;

    public CreateCategoryUseCaseTest(CreateCategoryFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldCreateCategory))]
    public async void ShouldCreateCategory()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateCategoryUseCase(repository.Object, unitOfWork.Object);
        var input = _fixture.GetCreateCategoryInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Insert(
                It.IsAny<Entity.Category>(),
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
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(ShouldCreateCategoryOnlyWithNameParam))]
    public async void ShouldCreateCategoryOnlyWithNameParam()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateCategoryUseCase(repository.Object, unitOfWork.Object);
        var input = new CreateCategoryInput(_fixture.GetValidName());

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Insert(
                It.IsAny<Entity.Category>(),
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
        output.Description.Should().Be("");
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(ShouldCreateCategoryOnlyWithNameAndDescriptionParam))]
    public async void ShouldCreateCategoryOnlyWithNameAndDescriptionParam()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var useCase = new CreateCategoryUseCase(repository.Object, unitOfWork.Object);
        var input = new CreateCategoryInput(
            _fixture.GetValidName(),
            _fixture.GetValidDescription()
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Insert(
                It.IsAny<Entity.Category>(),
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
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ShouldThrowExceptionWhenCannotCreateCategory))]
    [MemberData(
        nameof(CreateCategoryDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCategoryDataGenerator)
    )]
    public async void ShouldThrowExceptionWhenCannotCreateCategory(
        CreateCategoryInput input,
        string exceptionMessage
    )
    {
        var useCase = new CreateCategoryUseCase(
            _fixture.GetRepositoryMock().Object,
            _fixture.GetUnitOfWorkMock().Object
        );
        
        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);
        
        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}