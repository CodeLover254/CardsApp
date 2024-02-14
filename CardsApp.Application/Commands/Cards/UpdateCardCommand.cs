using System.Text.Json.Serialization;
using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Application.Commands.Cards;

public class UpdateCardCommand: CardRequest, IRequest<ApiResult<CardResponse?>>
{
    [JsonIgnore]
    public string Id { get; set; }
    public string Status { get; set; }
}

public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand, ApiResult<CardResponse?>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly CardAppDbContext _dbContext;
    private readonly CardEntityToResponseMapper _mapper;

    public UpdateCardCommandHandler(ICurrentUserProvider currentUserProvider, 
        CardAppDbContext dbContext, 
        CardEntityToResponseMapper mapper)
    {
        _currentUserProvider = currentUserProvider;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResult<CardResponse?>> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _dbContext.Cards.FirstOrDefaultAsync(c =>
            c.Id == request.Id && c.UserId == _currentUserProvider.UserId, cancellationToken: cancellationToken);
        if (card == null) return ResponseMessage<CardResponse?>.Error(null, "Card not found", ResponseCodes.NotFound);

        if (!string.IsNullOrEmpty(request.Name)) card.Name = request.Name; //important check to ensure the name is not empty

        card.Color = request.Color;
        card.Description = request.Description;
        card.Status = Enum.Parse<CardStatus>(request.Status);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return ResponseMessage<CardResponse?>.Success(_mapper.MapToResponse(card), 
                "Card successfully updated");
    }
}