using System.Text.Json.Serialization;
using CardsApp.Application.Interfaces;
using CardsApp.Application.Services;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Application.Commands.Cards;

public class DeleteCardCommand: IRequest<ApiResult<bool>>
{
    [JsonIgnore]
    public string Id { get; set; }
}

public class DeleteCardCommandHandler : BaseCardsQueryableBuilder, IRequestHandler<DeleteCardCommand, ApiResult<bool>>
{
    
    public DeleteCardCommandHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext)
    :base(dbContext, currentUserProvider)
    {
        
    }

    public async Task<ApiResult<bool>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await BuildQuery(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (card == null) return ResponseMessage<bool>.Error(false, "Card not found", ResponseCodes.NotFound);

        DbContext.Cards.Remove(card);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return ResponseMessage<bool>.Success(true, "Card successfully deleted");
    }
}