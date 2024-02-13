namespace CardsApp.Domain.Dto.Cards;

public class CardRequest
{
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}