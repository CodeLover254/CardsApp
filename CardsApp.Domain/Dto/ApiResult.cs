namespace CardsApp.Domain.Dto;

public class ApiResult<T>
{
    public T Result { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new ();
}