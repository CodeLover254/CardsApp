using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardsApp.Api.Controllers;

[Authorize]
public class BaseController : ControllerBase
{
    public IActionResult CustomResponse<T>(ApiResult<T> apiResult)
    {
        return apiResult.ResponseCode switch
        {
            ResponseCodes.Created => Created(string.Empty, apiResult),
            ResponseCodes.Ok => Ok(apiResult),
            ResponseCodes.Failed => BadRequest(apiResult),
            ResponseCodes.NotFound => NotFound(apiResult)
        };
    }
}