using CardsApp.Application.Commands.Cards;
using CardsApp.Application.Queries.Cards;
using CardsApp.Domain.Constants;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardsApp.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CardsController: BaseController
{
    private readonly IMediator _mediator;

    public CardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = $"{UserRoles.Member},{UserRoles.Admin}")]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResult<PaginatedResult<CardResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cards([FromQuery] CardsQuery query)
    {
        return CustomResponse(await _mediator.Send(query));
    }          
    
    [Authorize(Roles = $"{UserRoles.Member},{UserRoles.Admin}")]
    [HttpGet("{Id}")]
    [ProducesResponseType(typeof(ApiResult<CardResponse?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Card([FromRoute] CardQuery cardQuery)
    {
        return CustomResponse(await _mediator.Send(cardQuery));
    }

    [Authorize(Roles = UserRoles.Member)]
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResult<CardResponse?>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResult<>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardCommand command)
    {
        return CustomResponse(await _mediator.Send(command));
    }
    
    [Authorize(Roles = UserRoles.Member)]
    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(ApiResult<CardResponse?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCard([FromBody] UpdateCardCommand command,[FromRoute] string id)
    {
        command.Id = id;
        return CustomResponse(await _mediator.Send(command));
    }
    
    [Authorize(Roles = $"{UserRoles.Member},{UserRoles.Admin}")]
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCard([FromRoute] string id)
    {
        return CustomResponse(await _mediator.Send(new DeleteCardCommand{Id = id}));
    }
    
}