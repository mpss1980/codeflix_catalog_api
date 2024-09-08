using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace Codeflix.Catalog.UnitTest.Application.Category.CreateCategory;

public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryFixture();
        var invalidInputList = new List<object[]>
        {
            new object[]
            {
                fixture.GetInvalidInputWithShortName(),
                "Name should have at least 3 characters long"
            },
            new object[]
            {
                fixture.GetInvalidInputWithLongName(),
                "Name should be less or equal to 255 characters long"
            },
            new object[]
            {
            fixture.GetInvalidInputWithNullDescription(),
            "Description should not be null"
            },
            new object[]
            {
                fixture.GetInvalidInputWithLongDescription(),
                "Description should be less or equal to 10000 characters long"
        }
        };
        return invalidInputList;
    }
}