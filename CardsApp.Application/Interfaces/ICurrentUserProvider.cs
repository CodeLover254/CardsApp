namespace CardsApp.Application.Interfaces;

public interface ICurrentUserProvider
{
    public string? UserId { get; }
}