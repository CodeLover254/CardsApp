using CardsApp.Domain.Constants;
using CardsApp.Domain.Enums;

namespace CardsApp.Domain.Entities;

public class Card: BaseEntity
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public string Status { get; set; } = CardStatus.ToDo;
    public ApplicationUser User { get; set; }
}