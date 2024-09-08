using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.Category.UpdateCategory;

public class UpdateCategoryFixture : CategoryUseCaseBaseFixture
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new UpdateCategoryInput(
            id ?? Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            Faker.Random.Bool()
        );
    }

    public UpdateCategoryInput GetInvalidInputWithShortName()
    {
        var input = GetValidInput();
        input.Name = Faker.Random.String2(1, 2);
        return input;
    }

    public UpdateCategoryInput GetInvalidInputWithLongName()
    {
        var input = GetValidInput();
        input.Name = Faker.Random.String2(256);
        return input;
    }

    public UpdateCategoryInput GetInvalidInputWithLongDescription()
    {
        var input = GetValidInput();
        input.Description = Faker.Random.String2(10001);
        return input;
    }
}

[CollectionDefinition(nameof(UpdateCategoryFixture))]
public class UpdateCategoryFixtureCollection : ICollectionFixture<UpdateCategoryFixture>
{
}