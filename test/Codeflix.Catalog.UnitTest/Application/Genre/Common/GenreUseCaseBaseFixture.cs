using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Domain.Commons;
using Moq;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Genre.Common;

public abstract class GenreUseCaseBaseFixture : BaseFixture
{
    public Mock<IGenreRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new();

    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public Entity.Genre GetGenre(
        bool isActive = true,
        List<Guid>? categories = null)
    {
        var genre = new Entity.Genre(GetValidName(), isActive);

        if (categories is not null)
            foreach (var categoryId in categories)
                genre.AddCategory(categoryId);
        return genre;
    }
    
    public List<Entity.Genre> GetGenres(int count = 10)
        => Enumerable.Range(1, count).Select(_ =>
        {
            var genre = GetGenre();
            GetRandomIdsList().ForEach(genre.AddCategory);
            return genre;
        }).ToList();
    
   public List<Guid> GetRandomIdsList(int? count = null)
        => Enumerable.Range(1, count ?? (new Random()).Next(1, 10)).Select(_ => Guid.NewGuid()).ToList();

    private string GetValidCategoryName() => Faker.Random.String2(3, 255);

    private string GetValidCategoryDescription() => Faker.Random.String2(1, 10000);

    private Entity.Category GetCategory() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        Faker.Random.Bool()
    );
    
    public List<Entity.Category> GetCategories(int count = 5)
     => Enumerable.Range(0, count).Select(_ => GetCategory()).ToList();
}