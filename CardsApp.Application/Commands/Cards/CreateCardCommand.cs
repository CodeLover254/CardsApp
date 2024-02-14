using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Entities;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CardsApp.Application.Commands.Cards;

public class CreateCardCommand: CardRequest, IRequest<ApiResult<CardResponse?>>
{
}

public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, ApiResult<CardResponse?>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly CardAppDbContext _dbContext;
    private readonly ILogger<CreateCardCommandHandler> _logger;
    private readonly CardEntityToResponseMapper _mapper;

    public CreateCardCommandHandler(
        CardAppDbContext dbContext, 
        ILogger<CreateCardCommandHandler> logger, 
        ICurrentUserProvider currentUserProvider, 
        CardEntityToResponseMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _currentUserProvider = currentUserProvider;
        _mapper = mapper;
    }

    public async Task<ApiResult<CardResponse?>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        var card = new Card
        {
            Name = request.Name,
            Description = request.Description,
            Color = request.Color,
            UserId = _currentUserProvider.UserId!
        };

        _dbContext.Cards.Add(card);
        var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
        
        if (affectedRows > 0)
        {
            return ResponseMessage<CardResponse?>.Success(_mapper.MapToResponse(card), 
                "Card successfully created", 
                ResponseCodes.Created);
        }
        
        return ResponseMessage<CardResponse?>.Error(null, "Unable to create card");
    }
}