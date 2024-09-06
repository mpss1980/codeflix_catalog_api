namespace Codeflix.Catalog.Domain.Commons;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    
    protected Entity() => Id = Guid.NewGuid();
}