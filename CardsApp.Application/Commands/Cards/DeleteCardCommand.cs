using System.Text.Json.Serialization;
using CardsApp.Application.Interfaces;
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

public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, ApiResult<bool>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly CardAppDbContext _dbContext;

    public DeleteCardCommandHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext)
    {
        _currentUserProvider = currentUserProvider;
        _dbContext = dbContext;
    }

    public async Task<ApiResult<bool>> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id== request.Id && c.UserId == _currentUserProvider.UserId,
            cancellationToken: cancellationToken);

        if (card == null) return ResponseMessage<bool>.Error(false, "Card not found", ResponseCodes.NotFound);

        _dbContext.Cards.Remove(card);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return ResponseMessage<bool>.Success(true, "Card successfully deleted");
    }
}