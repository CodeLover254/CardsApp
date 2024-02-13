using System.Text.Json.Serialization;
using CardsApp.Domain.Enums;

namespace CardsApp.Domain.Dto.Results;

public class ApiResult<T>
{
    public T Result { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new ();
    
    [JsonIgnore]
    public ResponseCodes ResponseCode { get; set; }
}