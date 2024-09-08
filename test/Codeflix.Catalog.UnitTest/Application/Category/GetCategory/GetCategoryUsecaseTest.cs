using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using Codeflix.Catalog.UnitTest.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Category.GetCategory;

[Collection(nameof(GetCategoryFixture))]
public class GetCategoryUsecaseTest
{
    private readonly GetCategoryFixture _fixture;

    public GetCategoryUsecaseTest(GetCategoryFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = "ShouldGetACategory")]
    public async Task ShouldGetACategory()
    {
        var repository = _fixture.GetRepositoryMock();
        var category = _fixture.GetCategory();

        repository.Setup(repo => repo.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(category);

        var input = new GetCategoryInput(category.Id);
        var useCase = new GetCategoryUseCase(repository.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repository.Verify(
            repo => repo.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Id.Should().Be(category.Id);
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().BeSameDateAs(category.CreatedAt);
    }

    [Fact(DisplayName = "ShouldThrowNotFoundExceptionWhenCategoryIsNotFound")]
    public async Task ShouldThrowNotFoundExceptionWhenCategoryIsNotFound()
    {
        var repository = _fixture.GetRepositoryMock();
        var id = Guid.NewGuid();

        repository.Setup(repo => repo.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Category '{id}' not found")
        );

        var input = new GetCategoryInput(id);
        var useCase = new GetCategoryUseCase(repository.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();
        
        repository.Verify(
            repo => repo.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}