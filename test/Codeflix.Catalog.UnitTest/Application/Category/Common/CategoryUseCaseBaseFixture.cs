using Codeflix.Catalog.Domain.Repositories;
using Codeflix.Catalog.UnitTest.Domain.Commons;
using Moq;
using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.UnitTest.Application.Category.Common;

public abstract class CategoryUseCaseBaseFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidName() => Faker.Random.String2(3, 255);

    public string GetValidDescription() => Faker.Random.String2(1, 10000);
    
    public Entity.Category GetCategory() => new(
        GetValidName(),
        GetValidDescription(),
        Faker.Random.Bool()
    );
}