using CardsApp.Application.Commands.Cards;
using FluentValidation;

namespace CardsApp.Application.Validators.Cards;

public class UpdateCardCommandValidator: BaseCardValidator<UpdateCardCommand>
{
    public UpdateCardCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Color).Must(BeValidColor!)
            .When(x => !string.IsNullOrEmpty(x.Color))
            .WithMessage("Invalid hex color code");
        RuleFor(x => x.Status).IsInEnum();
    }
}