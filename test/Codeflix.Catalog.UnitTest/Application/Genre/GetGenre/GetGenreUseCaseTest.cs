using Codeflix.Catalog.Application.UseCases.Genre.GetGenre;
using Codeflix.Catalog.UnitTest.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Genre.GetGenre;

[Collection(nameof(GetGenreFixture))]
public class GetGenreUseCaseTest
{
    private readonly GetGenreFixture _fixture;

    public GetGenreUseCaseTest(GetGenreFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldGetAGenre))]
    public async Task ShouldGetAGenre()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var categories = _fixture.GetCategories();
        var genre = _fixture.GetGenre(categories: categories.Select(c => c.Id).ToList());

        repository.Setup(r => r.Get(
                It.Is<Guid>(x => x == genre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(genre);
        categoryRepository.Setup(x => x.GetListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(categories);

        var useCase = new GetGenreUsecase(repository.Object, categoryRepository.Object);
        var input = new GetGenreInput(genre.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(genre.Id);
        output.Name.Should().Be(genre.Name);
        output.IsActive.Should().Be(genre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(genre.CreatedAt);
        output.Categories.Should().HaveCount(genre.Categories.Count);
        foreach (var category in output.Categories)
        {
            var expectedCategory = categories.Single(x => x.Id == category.Id);
            category.Name.Should().Be(expectedCategory.Name);
        }

        repository.Verify(r => r.Get(
                It.Is<Guid>(x => x == genre.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldThrowNotFoundExceptionWhenGenreIsNotFound))]
    public async Task ShouldThrowNotFoundExceptionWhenGenreIsNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var id = Guid.NewGuid();

        repository.Setup(r => r.Get(
                It.Is<Guid>(x => x == id),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException(
                $"Genre '{id}' not found"
            ));

        var useCase = new GetGenreUsecase(repository.Object, categoryRepository.Object);
        var input = new GetGenreInput(id);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{id}' not found");

        repository.Verify(r => r.Get(
                It.Is<Guid>(x => x == id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}