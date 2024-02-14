using System.Text.RegularExpressions;
using FluentValidation;

namespace CardsApp.Application.Validators.Cards;

public class BaseCardValidator<T>: AbstractValidator<T>
{
    private readonly Regex _regex = new Regex("^#([A-Fa-f0-9]{6})$");
    
    protected bool BeValidColor(string color)
    {
        return _regex.IsMatch(color);
    }
}