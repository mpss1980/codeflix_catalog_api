using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using Codeflix.Catalog.Domain.Commons.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Category.ListCategories;

[Collection(nameof(ListCategoriesFixture))]
public class ListCategoriesUseCaseTest
{
    private readonly ListCategoriesFixture _fixture;

    public ListCategoriesUseCaseTest(ListCategoriesFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldListCategories))]
    public async Task ShouldListCategories()
    {
        var categories = _fixture.GetCategories();
        var repository = _fixture.GetRepositoryMock();
        var input = _fixture.GetListCategoriesInput();

        var repositoryFoundOutput = new SearchOutput<Entity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<Entity.Category>)categories,
            total: new Random().Next(50, 200)
        );
        repository.Setup(repo => repo.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositoryFoundOutput);

        var useCase = new ListCategoriesUseCase(repository.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositoryFoundOutput.CurrentPage);
        output.PerPage.Should().Be(repositoryFoundOutput.PerPage);
        output.Total.Should().Be(repositoryFoundOutput.Total);
        output.Items.Should().HaveCount(repositoryFoundOutput.Items.Count);

        ((List<CategoryOutput>)output.Items).ForEach(outputItem =>
        {
            var categoryFromRepo = repositoryFoundOutput.Items.FirstOrDefault(x => x.Id == outputItem.Id);
            categoryFromRepo.Should().NotBeNull();
            outputItem.Name.Should().Be(categoryFromRepo!.Name);
            outputItem.Description.Should().Be(categoryFromRepo!.Description);
            outputItem.IsActive.Should().Be(categoryFromRepo!.IsActive);
            outputItem.CreatedAt.Should().Be(categoryFromRepo!.CreatedAt);
        });

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ShouldListCategoriesWithEmptyList))]
    public async Task ShouldListCategoriesWithEmptyList()
    {
        var repository = _fixture.GetRepositoryMock();
        var input = _fixture.GetListCategoriesInput();

        var repositoryFoundOutput = new SearchOutput<Entity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<Entity.Category>().AsReadOnly(),
            total: 0
        );
        repository.Setup(repo => repo.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositoryFoundOutput);

        var useCase = new ListCategoriesUseCase(repository.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositoryFoundOutput.CurrentPage);
        output.PerPage.Should().Be(repositoryFoundOutput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(ShouldListCategoriesWithDifferentSearch))]
    [MemberData(
        nameof(ListCategoriesDataGenerator.GetInputsWithoutAllParameters),
        MemberType = typeof(ListCategoriesDataGenerator)
    )]
    public async Task ShouldListCategoriesWithDifferentSearch(
        ListCategoriesInput input
    )
    {
        var categories = _fixture.GetCategories();
        var repository = _fixture.GetRepositoryMock();

        var repositoryFoundOutput = new SearchOutput<Entity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<Entity.Category>)categories,
            total: new Random().Next(50, 200)
        );
        repository.Setup(repo => repo.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(repositoryFoundOutput);

        var useCase = new ListCategoriesUseCase(repository.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositoryFoundOutput.CurrentPage);
        output.PerPage.Should().Be(repositoryFoundOutput.PerPage);
        output.Total.Should().Be(repositoryFoundOutput.Total);
        output.Items.Should().HaveCount(repositoryFoundOutput.Items.Count);

        ((List<CategoryOutput>)output.Items).ForEach(outputItem =>
        {
            var categoryFromRepo = repositoryFoundOutput.Items.FirstOrDefault(x => x.Id == outputItem.Id);
            categoryFromRepo.Should().NotBeNull();
            outputItem.Name.Should().Be(categoryFromRepo!.Name);
            outputItem.Description.Should().Be(categoryFromRepo!.Description);
            outputItem.IsActive.Should().Be(categoryFromRepo!.IsActive);
            outputItem.CreatedAt.Should().Be(categoryFromRepo!.CreatedAt);
        });

        repository.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}