namespace CardsApp.Domain.Dto.Cards;

public class BasePaginatedItemsQuery
{
    public int Index { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}