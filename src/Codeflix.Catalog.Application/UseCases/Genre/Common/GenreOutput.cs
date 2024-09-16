using Entity = Codeflix.Catalog.Domain.Entities;

namespace Codeflix.Catalog.Application.UseCases.Genre.Common;

public class GenreOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<GenreOutputCategory> Categories { get; set; }

    public GenreOutput(Guid id, string name, bool isActive, DateTime createdAt,
        IReadOnlyList<GenreOutputCategory> categories)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }

    public static GenreOutput FromGenre(Entity.Genre genre)
    {
        return new GenreOutput(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(c => new GenreOutputCategory(c)).ToList().AsReadOnly()
        );
    }
}

public class GenreOutputCategory
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public GenreOutputCategory(Guid id, string? name = null)
    {
        Id = id;
        Name = name;
    }
}