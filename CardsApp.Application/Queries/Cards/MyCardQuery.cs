using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Application.Queries.Cards;

public class MyCardQuery: IRequest<ApiResult<CardResponse?>>
{
    public string Id { get; set; }
}

public class MyCardQueryHandler : IRequestHandler<MyCardQuery, ApiResult<CardResponse?>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly CardAppDbContext _dbContext;
    private readonly CardEntityToResponseMapper _mapper;

    public MyCardQueryHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext, CardEntityToResponseMapper mapper)
    {
        _currentUserProvider = currentUserProvider;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResult<CardResponse?>> Handle(MyCardQuery request, CancellationToken cancellationToken)
    {
        var card = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id== request.Id && c.UserId == _currentUserProvider.UserId,
            cancellationToken: cancellationToken);

        if (card == null) return ResponseMessage<CardResponse?>.Error(null, "Card not found", ResponseCodes.NotFound);

        return ResponseMessage<CardResponse?>.Success(_mapper.MapToResponse(card), "success");
    }
}