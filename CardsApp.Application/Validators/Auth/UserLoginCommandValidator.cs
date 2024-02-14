using CardsApp.Application.Commands.Auth;
using FluentValidation;

namespace CardsApp.Application.Validators.Auth;

public class UserLoginCommandValidator: AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}