using Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;
using Codeflix.Catalog.UnitTest.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using Xunit.Sdk;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Genre.DeleteGenre;

[Collection(nameof(DeleteGenreFixture))]
public class DeleteGenreUseCaseTest
{
    private readonly DeleteGenreFixture _fixture;

    public DeleteGenreUseCaseTest(DeleteGenreFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldDeleteAGenre))]
    public async Task ShouldDeleteAGenre()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var genre = _fixture.GetGenre();

        repository.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(genre);

        var input = new DeleteGenreInput(genre.Id);
        var usecase = new DeleteGenreUseCase(repository.Object, unitOfWork.Object);

        await usecase.Handle(input, CancellationToken.None);

        repository.Verify(x => x.Get(
            It.Is<Guid>(id => id == genre.Id),
            It.IsAny<CancellationToken>()
        ), Times.Once);
        repository.Verify(
            x => x.Delete(
                It.Is<Entity.Genre>(g => g.Id == genre.Id),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        unitOfWork.Verify(
            x => x.Commit(It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldThrowNotFoundExceptionWhenGenreNotFound))]
    public async Task ShouldThrowNotFoundExceptionWhenGenreNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var idToDelete = Guid.NewGuid();
        var input = new DeleteGenreInput(idToDelete);

        repository.Setup(x => x.Get(
            It.Is<Guid>(g => g == idToDelete),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException(
            $"Genre '{idToDelete}' not found"));

        var usecase = new DeleteGenreUseCase(repository.Object, unitOfWork.Object);

        var action = async () => await usecase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{idToDelete}' not found");

        repository.Verify(x => x.Get(
            It.Is<Guid>(id => id == idToDelete),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}