using CardsApp.Domain.Enums;

namespace CardsApp.Domain.Dto.Results;

public static class ResponseMessage<T>
{
    public static ApiResult<T> Success(T data, string message,ResponseCodes responseCode=ResponseCodes.Ok)
    {
        return new ApiResult<T>
        {
            Message = message,
            Result = data,
            ResponseCode = responseCode
        };
    }

    public static ApiResult<T> Error(T data, 
        string message, 
        ResponseCodes responseCode=ResponseCodes.Failed,
        IEnumerable<string> errors=null)
    {
        return new ApiResult<T>
        {
            Message = message,
            Result = data,
            ResponseCode = responseCode,
            Errors = new List<string>(errors)
        };
    }

}