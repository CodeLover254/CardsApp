using CardsApp.Application.Commands.Auth;
using CardsApp.Domain.Dto.Auth;
using CardsApp.Domain.Dto.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardsApp.Api.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController: BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResult<UserLoginResponse?>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<UserLoginResponse?>),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand command)
    {
        return CustomResponse(await _mediator.Send(command));
    }
}