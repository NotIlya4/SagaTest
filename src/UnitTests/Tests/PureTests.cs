using FluentValidation;
using MoneyService.Models;

namespace UnitTests;

public class PureTests
{
    [Fact]
    public async Task Test()
    {
        var user = new User(0, -1, false);
        var userValidator = new UserValidator();

        var validationResult = await userValidator.ValidateAsync(user);
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Money).Must(x => x >= 0);
    }
}