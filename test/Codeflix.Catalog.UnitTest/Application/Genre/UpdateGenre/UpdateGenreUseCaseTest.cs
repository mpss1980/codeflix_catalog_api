using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.UnitTest.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Genre.UpdateGenre;

[Collection(nameof(UpdateGenreFixture))]
public class UpdateGenreUseCaseTest
{
    private readonly UpdateGenreFixture _fixture;

    public UpdateGenreUseCaseTest(UpdateGenreFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldUpdateGenre))]
    public async Task ShouldUpdateGenre()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre();
        var nameToUpdate = _fixture.GetValidName();
        var newIsActive = !genre.IsActive;

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate, newIsActive);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(nameToUpdate);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        output.Categories.Should().BeEquivalentTo(genre.Categories);

        repository.Verify(x => x.Update(
            It.Is<DomainEntity.Genre>(x => x.Id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldThrowExceptionWhenGenreNotFound))]
    public async Task ShouldThrowExceptionWhenGenreNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var id = Guid.NewGuid();

        repository.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException(
            $"Genre {id} not found"
        ));

        var useCase = new UpdateGenreUseCase(repository.Object, _fixture.GetCategoryRepositoryMock().Object,
            _fixture.GetUnitOfWorkMock().Object);
        var input = new UpdateGenreInput(id, _fixture.GetValidName(), true);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>().WithMessage(
            $"Genre {id} not found"
        );
    }

    [Theory(DisplayName = nameof(ShouldThrowExceptionWhenNameIsInvalid))]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ShouldThrowExceptionWhenNameIsInvalid(string? name)
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre();

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        var useCase = new UpdateGenreUseCase(repository.Object, _fixture.GetCategoryRepositoryMock().Object,
            unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, name!);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<EntityValidationException>().WithMessage(
            "Name should not be null or empty"
        );
    }

    [Theory(DisplayName = nameof(ShouldUpdateOnlyName))]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldUpdateOnlyName(bool isActive)
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre(isActive: isActive);
        var nameToUpdate = _fixture.GetValidName();

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(nameToUpdate);
        output.IsActive.Should().Be(isActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        output.Categories.Should().BeEquivalentTo(genre.Categories);

        repository.Verify(x => x.Update(
            It.Is<DomainEntity.Genre>(x => x.Id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldUpdateGenreWhenAddingCategoriesIds))]
    public async Task ShouldUpdateGenreWhenAddingCategoriesIds()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre();
        var nameToUpdate = _fixture.GetValidName();
        var newIsActive = !genre.IsActive;
        var categoriesIds = _fixture.GetRandomIdsList();

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        categoryRepository.Setup(x => x.GetIdListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(categoriesIds);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate, newIsActive, categoriesIds);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(nameToUpdate);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        categoriesIds.ForEach(
            id => output.Categories.Should().Contain(relation => relation.Id == id));

        repository.Verify(x => x.Update(
            It.Is<DomainEntity.Genre>(x => x.Id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact(DisplayName = nameof(ShouldUpdateGenreWhenReplacingCategoriesIds))]
    public async Task ShouldUpdateGenreWhenReplacingCategoriesIds()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre(
            categories: _fixture.GetRandomIdsList()
        );
        var categoriesIds = _fixture.GetRandomIdsList();
        var nameToUpdate = _fixture.GetValidName();
        var newIsActive = !genre.IsActive;

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        categoryRepository.Setup(x => x.GetIdListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(categoriesIds);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate, newIsActive, categoriesIds);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(nameToUpdate);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        output.Categories.Should().HaveCount(categoriesIds.Count);
        categoriesIds.ForEach(
            id => output.Categories.Should().Contain(relation => relation.Id == id));

        repository.Verify(x => x.Update(
            It.Is<DomainEntity.Genre>(x => x.Id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldThrowExceptionWhenCategoryIsNotFound))]
    public async Task ShouldThrowExceptionWhenCategoryIsNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre(
            categories: _fixture.GetRandomIdsList()
        );
        var categoriesIds = _fixture.GetRandomIdsList(10);
        var returnedByCategoryRepoList = categoriesIds.GetRange(0, categoriesIds.Count - 2);
        var idsReturnedByCategoryRepo = categoriesIds.GetRange(categoriesIds.Count - 2, 2);
        var nameToUpdate = _fixture.GetValidName();
        var newIsActive = !genre.IsActive;

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        categoryRepository.Setup(x => x.GetIdListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(returnedByCategoryRepoList);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate, newIsActive, categoriesIds);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        var notFoundAsString = string.Join(", ", idsReturnedByCategoryRepo);
        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage(
            $"Related category id (or ids) not found: {notFoundAsString}"
        );
    }
    
    [Fact(DisplayName = nameof(ShouldUpdateGenreWithoutCategoriesIds))]
    public async Task ShouldUpdateGenreWithoutCategoriesIds()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryIds = _fixture.GetRandomIdsList();
        var genre = _fixture.GetGenre(
            categories: categoryIds
        );
        var nameToUpdate = _fixture.GetValidName();
        var newIsActive = !genre.IsActive;

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate, newIsActive);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(nameToUpdate);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        output.Categories.Should().HaveCount(categoryIds.Count);
        categoryIds.ForEach(
            id => output.Categories.Should().Contain(relation => relation.Id == id));

        repository.Verify(x => x.Update(
            It.Is<DomainEntity.Genre>(x => x.Id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact(DisplayName = nameof(ShouldUpdateGenreWithEmptyCategoriesIds))]
    public async Task ShouldUpdateGenreWithEmptyCategoriesIds()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryIds = _fixture.GetRandomIdsList();
        var genre = _fixture.GetGenre(
            categories: categoryIds
        );
        var nameToUpdate = _fixture.GetValidName();
        var newIsActive = !genre.IsActive;

        repository.Setup(x => x.Get(
            It.Is<Guid>(x => x == genre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        var useCase = new UpdateGenreUseCase(repository.Object, categoryRepository.Object, unitOfWork.Object);
        var input = new UpdateGenreInput(genre.Id, nameToUpdate, newIsActive, new List<Guid>());

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(nameToUpdate);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        output.Categories.Should().BeEmpty();

        repository.Verify(x => x.Update(
            It.Is<DomainEntity.Genre>(x => x.Id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}