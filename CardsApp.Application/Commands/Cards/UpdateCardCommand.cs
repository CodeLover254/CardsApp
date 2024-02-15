using System.Text.Json.Serialization;
using CardsApp.Application.Interfaces;
using CardsApp.Application.Services;
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

public class UpdateCardCommandHandler : BaseCardsQueryableBuilder, IRequestHandler<UpdateCardCommand, ApiResult<CardResponse?>>
{
   
    private readonly CardEntityToResponseMapper _mapper;

    public UpdateCardCommandHandler(ICurrentUserProvider currentUserProvider, 
        CardAppDbContext dbContext, 
        CardEntityToResponseMapper mapper)
    :base(dbContext, currentUserProvider)
    {
      
        _mapper = mapper;
    }

    public async Task<ApiResult<CardResponse?>> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var card = await BuildQuery(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (card == null) return ResponseMessage<CardResponse?>.Error(null, "Card not found", ResponseCodes.NotFound);

        if (!string.IsNullOrEmpty(request.Name)) card.Name = request.Name; //important check to ensure the name is not empty

        card.Color = request.Color;
        card.Description = request.Description;
        
        //status cannot be cleared out
        if(!string.IsNullOrEmpty(request.Status))
            card.Status =request.Status;
        
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return ResponseMessage<CardResponse?>.Success(_mapper.MapToResponse(card), 
                "Card successfully updated");
    }
}