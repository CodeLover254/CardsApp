namespace CardsApp.Domain.Dto.Results;

public class PaginatedResult<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int Total { get; set; }
    public int PageSize { get; set; }
    public int Index { get; set; }
}