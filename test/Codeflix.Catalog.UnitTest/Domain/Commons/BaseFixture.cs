using Bogus;

namespace Codeflix.Catalog.UnitTest.Domain.Commons;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }
    
    protected BaseFixture() => Faker = new Faker("pt_BR");
}