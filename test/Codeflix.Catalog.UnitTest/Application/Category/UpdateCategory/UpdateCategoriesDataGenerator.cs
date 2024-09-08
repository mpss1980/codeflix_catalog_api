namespace Codeflix.Catalog.UnitTest.Application.Category.UpdateCategory;

public class UpdateCategoriesDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryFixture();
        var categories = new List<object[]>();

        for (var i = 0; i < times; i++)
        {
            var category = fixture.GetCategory();
            var input = fixture.GetValidInput(category.Id);
            categories.Add(new object[] { category, input });
        }
        return categories;
    }
    
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new UpdateCategoryFixture();
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
                fixture.GetInvalidInputWithLongDescription(),
                "Description should be less or equal to 10000 characters long"
            }
        };
        return invalidInputList;
    }
}