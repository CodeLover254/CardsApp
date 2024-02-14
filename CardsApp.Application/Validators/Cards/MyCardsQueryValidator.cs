using CardsApp.Application.Queries.Cards;
using CardsApp.Domain.Enums;
using FluentValidation;

namespace CardsApp.Application.Validators.Cards;

public class MyCardsQueryValidator: AbstractValidator<MyCardsQuery>
{
    public MyCardsQueryValidator()
    {
        RuleFor(x=>x.Index).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        RuleFor(x => x.FilterBy).IsInEnum()
            .When(x => x.FilterBy != null)
            .WithMessage("Invalid FilterBy value");
        RuleFor(x => x.SortBy).IsInEnum()
            .When(x => x.SortBy != null)
            .WithMessage("Invalid SortBy value");;
        RuleFor(x => x.SearchTerm).NotEmpty()
            .When(x => x.FilterBy != null);
        RuleFor(x => x.SearchTerm).Must(BeValidStatus!)
            .When(x => x.FilterBy != null && x.FilterBy == CardFilterables.Status)
            .WithMessage("Invalid FilterBy status");
        RuleFor(x => x.SearchTerm).Must(BeValidDate!)
            .When(x => x.FilterBy != null && x.FilterBy == CardFilterables.DateCreated)
            .WithMessage("Invalid FilterBy date format. Please use yyyy-mm-dd");
    }

    private bool BeValidStatus(string searchTerm)
    {
        return Enum.TryParse(typeof(CardStatus), searchTerm, out _);
    }
    
    private bool BeValidDate(string searchTerm)
    {
        return DateTime.TryParse(searchTerm, out _);
    }
}