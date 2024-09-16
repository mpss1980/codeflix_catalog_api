using Codeflix.Catalog.Domain.Commons;
using Codeflix.Catalog.Domain.Validations;

namespace Codeflix.Catalog.Domain.Entities;

public class Genre : Aggregate
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<Guid> Categories => _categories.AsReadOnly();

    private List<Guid> _categories;

    public Genre(string name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        _categories = new List<Guid>();
        Validate();
    }
    
    public void Activate()
    {
        IsActive = true;
        Validate();
    }
    
    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }
    
    public void AddCategory(Guid categoryId)
    {
        _categories.Add(categoryId);
        Validate();
    }
    
    public void RemoveCategory(Guid categoryId)
    {
        _categories.Remove(categoryId);
        Validate();
    }
    
    public void RemoveAllCategories()
    {
        _categories.Clear();
        Validate();
    }
    
    public void Update(string name)
    {
        Name = name;
        Validate();
    }

    private void Validate() => DomainValidation.NotNullOrEmpty(Name, nameof(Name));
}