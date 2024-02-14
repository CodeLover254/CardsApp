using CardsApp.Application.Commands.Cards;
using CardsApp.Domain.Enums;
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
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
        RuleFor(x => x.Status).Must(IsValidEnum).WithMessage("Invalid status value");
    }

    private bool IsValidEnum(string status)
    {
        return Enum.TryParse<CardStatus>(status, out _);
    }
}