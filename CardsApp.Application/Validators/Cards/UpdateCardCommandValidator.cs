using CardsApp.Application.Commands.Cards;
using CardsApp.Domain.Constants;
using CardsApp.Domain.Enums;
using FluentValidation;

namespace CardsApp.Application.Validators.Cards;

public class UpdateCardCommandValidator: BaseCardValidator<UpdateCardCommand>
{
    private readonly string[] _validStatus = [CardStatus.ToDo, CardStatus.InProgress, CardStatus.Done];
    public UpdateCardCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Color).Must(BeValidColor!)
            .When(x => !string.IsNullOrEmpty(x.Color))
            .WithMessage("Invalid hex color code");
        RuleFor(x => x.Status).Must(BeValidStatus)
            .When(x=>!string.IsNullOrEmpty(x.Status))
            .WithMessage($"Invalid status. Allowed values are [{string.Join(',',_validStatus)}]");
    }

    private bool BeValidStatus(string searchTerm)
    {
        return _validStatus.Contains(searchTerm);
    }
}