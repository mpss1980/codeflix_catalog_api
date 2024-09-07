using Bogus;
using Codeflix.Catalog.Domain.Exceptions;
using Codeflix.Catalog.Domain.Validations;
using FluentAssertions;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Domain.Validations;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker("pt_BR");
    
    [Fact(DisplayName = nameof(ShouldNotThrowExceptionWhenTargetIsNotNull))]
    public void ShouldNotThrowExceptionWhenTargetIsNotNull()
    {
        var target = Faker.Random.Word();
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.NotNull(target, field);
        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(ShouldThrowExceptionWhenTargetIsNull))]
    public void ShouldThrowExceptionWhenTargetIsNull()
    {
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.NotNull(null, field);
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{field} should not be null");
    }
    
    [Fact(DisplayName = nameof(ShouldNotThrowExceptionWhenTargetIsNotEmpty))]
    public void ShouldNotThrowExceptionWhenTargetIsNotEmpty()
    {
        var target = Faker.Random.Word();
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.NotNullOrEmpty(target, field);
        action.Should().NotThrow();
    }
    
    [Theory(DisplayName = nameof(ShouldThrowExceptionWhenTargetEmptyBlankOrNull))]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ShouldThrowExceptionWhenTargetEmptyBlankOrNull(string? target)
    {
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.NotNullOrEmpty(target, field);
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{field} should not be null or empty");
    }
    
    [Fact(DisplayName = nameof(ShouldNotThrowExceptionWhenTargetLengthIsGreaterThanMinLength))]
    public void ShouldNotThrowExceptionWhenTargetLengthIsGreaterThanMinLength()
    {
        var target = Faker.Random.String2(3, 255);
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.MinLength(target, 3, field);
        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(ShouldThrowExceptionWhenTargetLengthIsSmallerThanMinLength))]
    public void ShouldThrowExceptionWhenTargetLengthIsSmallerThanMinLength()
    {
        var target = Faker.Random.String2(2);
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.MinLength(target, 3, field);
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{field} should have at least 3 characters long");
    }
    
    [Fact(DisplayName = nameof(ShouldNotThrowExceptionWhenTargetLengthIsSmallerThanMaxLength))]
    public void ShouldNotThrowExceptionWhenTargetLengthIsSmallerThanMaxLength()
    {
        var target = Faker.Random.String2(3, 255);
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.MaxLength(target, 255, field);
        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(ShouldThrowExceptionWhenTargetLengthIsGreaterThanMaxLength))]
    public void ShouldThrowExceptionWhenTargetLengthIsGreaterThanMaxLength()
    {
        var target = Faker.Random.String2(256);
        var field = Faker.Random.Word();
        
        var action = () => DomainValidation.MaxLength(target, 255, field);
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{field} should be less or equal to 255 characters long");
    }
}