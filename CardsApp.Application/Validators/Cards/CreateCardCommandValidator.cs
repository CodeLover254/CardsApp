using System.Text.RegularExpressions;
using CardsApp.Application.Commands.Cards;
using FluentValidation;

namespace CardsApp.Application.Validators.Cards;

public class CreateCardCommandValidator: BaseCardValidator<CreateCardCommand>
{
    public CreateCardCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Color).Must(BeValidColor!)
            .When(x => !string.IsNullOrEmpty(x.Color))
            .WithMessage("Invalid hex color code");
    }

    
}