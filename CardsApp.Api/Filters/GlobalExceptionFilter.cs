using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CardsApp.Api.Filters;

public class GlobalExceptionFilter: IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var traceId = Guid.NewGuid().ToString();
        ApiResult<string> apiResult = new ApiResult<string>
        {
            Result = null,
            ResponseCode = ResponseCodes.Failed
        };

        if (context.Exception.GetType() == typeof(ValidationException))
        {
            var validationException = context.Exception as ValidationException;
            apiResult.Errors = validationException!.Errors.Select(x => x.ErrorMessage).ToList();
            apiResult.Message = "Invalid inputs.";
            context.Result = new BadRequestObjectResult(apiResult);
        }
        else
        {
            _logger.LogError(context.Exception,"An exception occurred during a request with message {message}. Trace Id: {traceId}",context.Exception.Message,traceId);
            apiResult.Message = $"Something went wrong. Please try again later. Trace Id: {traceId}";
            context.Result = new BadRequestObjectResult(apiResult);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        context.ExceptionHandled = true;
    }
}