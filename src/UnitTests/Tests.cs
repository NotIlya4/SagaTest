using Bogus;
using FluentValidation;
using FluentValidation.Validators;

namespace UnitTests;

public class Tests
{
    private readonly Faker<Address> _addressFaker;
    private readonly Faker<Customer> _customerFaker;

    public Tests()
    {
        _addressFaker = new Faker<Address>("ru")
            .StrictMode(true)
            .UseSeed(10)
            .RuleFor(x => x.Town, x => x.Address.City())
            .RuleFor(x => x.Country, x => x.Address.Country())
            .RuleFor(x => x.Postcode, x => x.Address.ZipCode());

        _customerFaker = new Faker<Customer>("ru")
            .StrictMode(true)
            .UseSeed(10)
            .RuleFor(x => x.Id, x => x.IndexFaker)
            .RuleFor(x => x.Forename, x => x.Person.FirstName)
            .RuleFor(x => x.Surname, x => x.Person.LastName)
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.Forename, x.Surname, "gmail.com"))
            .RuleFor(x => x.Discount, x => x.Finance.Amount(0, 10))
            .RuleFor(x => x.Address, () => _addressFaker.Generate());
    }

    private readonly CustomerValidator _validator = new CustomerValidator();
    
    [Fact]
    public async Task Test()
    {
        Customer customer = _customerFaker.Generate()!;

        var result = await _validator.ValidateAsync(customer);
    }
}

public record Customer 
{
    public required int Id { get; init; }
    public required string Forename { get; init; }
    public required string Surname { get; init; }
    public required string Email { get; init; }
    public required decimal Discount { get; init; }
    public required Address Address { get; init; }
}

public record Address
{
    public required string Town { get; init; }
    public required string Country { get; init; }
    public required string Postcode { get; init; }
}

public class Order 
{
    public required double Total { get; init; }
}

public class CustomerValidator : AbstractValidator<Customer> 
{
    public CustomerValidator()
    {
        RuleFor(x => x.Forename).NotEqual("Ilya");
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Address).SetValidator(new AddressValidator());
    }
}

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Town).MinimumLength(3);
        RuleFor(x => x.Country).MaximumLength(1);
    }
}