using CardsApp.Application.Queries.Cards;
using CardsApp.Domain.Constants;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardsApp.Api.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("api/[controller]")]
public class AdminController: BaseController
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("cards")]
    [ProducesResponseType(typeof(ApiResult<PaginatedResult<CardResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCards([FromQuery] AdminCardsQuery query)
    {
        return CustomResponse(await _mediator.Send(query));
    }
}