using CardsApp.Domain.Enums;

namespace CardsApp.Domain.Dto.Cards;

public class BasePaginatedItemsQuery
{
    public int Index { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public CardFilterables? FilterBy { get; set; }
    public string? SearchTerm { get; set; }
    public CardFilterables? SortBy { get; set; }
}