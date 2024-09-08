using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Category.CreateCategory;

public class CreateCategoryFixture : CategoryUseCaseBaseFixture
{
    public CreateCategoryInput GetCreateCategoryInput()
     => new (
         GetValidName(),
         GetValidDescription(),
         Faker.Random.Bool()
     );
    
    public CreateCategoryInput GetInvalidInputWithShortName()
     => new (
         Faker.Random.String2(1, 2),
         GetValidDescription(),
         Faker.Random.Bool()
     );
    
    public CreateCategoryInput GetInvalidInputWithLongName() 
     => new (
         Faker.Random.String2(256),
         GetValidDescription(),
         Faker.Random.Bool()
     );
    
    public CreateCategoryInput GetInvalidInputWithLongDescription() 
     => new (
         GetValidName(),
         Faker.Random.String2(10001),
         Faker.Random.Bool()
     );

    public CreateCategoryInput GetInvalidInputWithNullDescription()
    {
        var categoryInput = GetCreateCategoryInput();
        categoryInput.Description = null!;
        return categoryInput;
    }

}

[CollectionDefinition(nameof(CreateCategoryFixture))]
public class CreateCategoryFixtureCollection : ICollectionFixture<CreateCategoryFixture>
{
}