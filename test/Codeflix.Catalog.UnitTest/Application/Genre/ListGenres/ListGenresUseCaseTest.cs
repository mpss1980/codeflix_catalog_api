using Codeflix.Catalog.Application.UseCases.Genre.ListGenres;
using Codeflix.Catalog.Domain.Commons;
using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;
using EntityDomain = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Genre.ListGenres;

[Collection(nameof(ListGenresFixture))]
public class ListGenreUseCaseTest
{
    private readonly ListGenresFixture _fixture;

    public ListGenreUseCaseTest(ListGenresFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldListGenresPaginated))]
    public async Task ShouldListGenresPaginated()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var genres = _fixture.GetGenres();
        var input = _fixture.GetListGenreInput();

        var outputRepositorySearch = new SearchOutput<EntityDomain.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: genres,
            total: new Random().Next(50, 200)
        );

        repository.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListGenresUseCase(repository.Object, categoryRepository.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        foreach (var outputItem in output.Items)
        {
            var genre = genres.FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(genre!.Name);
            outputItem.IsActive.Should().Be(genre.IsActive);
            outputItem.CreatedAt.Should().Be(genre.CreatedAt);
            outputItem.Categories.Should().HaveCount(genre.Categories.Count);
            foreach (var expectedId in genre.Categories)
                outputItem.Categories.Should().Contain(relation => relation.Id == expectedId);
        }

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        var expectedIds = genres.SelectMany(genre => genre.Categories).Distinct().ToList();
        categoryRepository.Verify(x => x.GetListByIds(
            It.Is<List<Guid>>(ids =>
                ids.All(id => expectedIds.Contains(id)
                              && ids.Count == expectedIds.Count
                )),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldListGenresEvenOnEmptyList))]
    public async Task ShouldListGenresEvenOnEmptyList()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var input = _fixture.GetListGenreInput();

        var outputRepositorySearch = new SearchOutput<EntityDomain.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<EntityDomain.Genre>(),
            total: new Random().Next(50, 200)
        );

        repository.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListGenresUseCase(repository.Object, categoryRepository.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        categoryRepository.Verify(x => x.GetListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        ), Times.Never);
    }
    
    [Fact(DisplayName = nameof(ShouldListGenresUsingDefaultValues))]
    public async Task ShouldListGenresUsingDefaultValues()
    {
        var repository = _fixture.GetRepositoryMock();
        var categoryRepository = _fixture.GetCategoryRepositoryMock();
        var input = new ListGenresInput();

        var outputRepositorySearch = new SearchOutput<EntityDomain.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<EntityDomain.Genre>(),
            total: 0
        );

        repository.Setup(x => x.Search(
            It.IsAny<SearchInput>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new ListGenresUseCase(repository.Object, categoryRepository.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(searchInput =>
                searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.Search == input.Search
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        categoryRepository.Verify(x => x.GetListByIds(
            It.IsAny<List<Guid>>(),
            It.IsAny<CancellationToken>()
        ), Times.Never);
    }
}