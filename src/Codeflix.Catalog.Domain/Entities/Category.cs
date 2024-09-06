using Codeflix.Catalog.Domain.Commons;

namespace Codeflix.Catalog.Domain.Entities;

public class Category : Aggregate
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true) : base()
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
    }
}