using System.Security.Claims;
using CardsApp.Domain;
using CardsApp.Domain.Dto;
using CardsApp.Domain.Dto.Cards;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CardsApp.Application.Commands.Cards;

public class CreateCardCommand: IRequest<ApiResult<CreateCardResponse?>>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, ApiResult<CreateCardResponse?>>
{
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly CardAppDbContext _dbContext;
    private readonly ILogger<CreateCardCommandHandler> _logger;

    public CreateCardCommandHandler(ClaimsPrincipal claimsPrincipal,
        CardAppDbContext dbContext, 
        ILogger<CreateCardCommandHandler> logger)
    {
        _claimsPrincipal = claimsPrincipal;
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task<ApiResult<CreateCardResponse?>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}