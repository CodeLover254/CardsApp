using CardsApp.Application.Interfaces;
using CardsApp.Application.Services;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Application.Queries.Cards;

public class CardQuery: IRequest<ApiResult<CardResponse?>>
{
    public string Id { get; set; }
}

public class CardQueryHandler : BaseCardsQueryableBuilder, IRequestHandler<CardQuery, ApiResult<CardResponse?>>
{
    private readonly CardEntityToResponseMapper _mapper;

    public CardQueryHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext, CardEntityToResponseMapper mapper)
    :base(dbContext, currentUserProvider)
    {
        _mapper = mapper;
    }

    public async Task<ApiResult<CardResponse?>> Handle(CardQuery request, CancellationToken cancellationToken)
    {
        var card = await BuildQuery(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (card == null) return ResponseMessage<CardResponse?>.Error(null, "Card not found", ResponseCodes.NotFound);

        return ResponseMessage<CardResponse?>.Success(_mapper.MapToResponse(card), "success");
    }
}