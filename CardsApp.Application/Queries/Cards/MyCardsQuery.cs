using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Mappers.Cards;
using MediatR;


namespace CardsApp.Application.Queries.Cards;

public class MyCardsQuery: BasePaginatedItemsQuery, IRequest<ApiResult<PaginatedResult<CardResponse>>>
{
   
}

public class MyCardsQueryHandler : BaseCardsQueryHandler, IRequestHandler<MyCardsQuery, ApiResult<PaginatedResult<CardResponse>>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly CardAppDbContext _dbContext;

    public MyCardsQueryHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext, CardEntityToResponseMapper mapper)
    :base(mapper)
    {
        _currentUserProvider = currentUserProvider;
        _dbContext = dbContext;
    }

    public async Task<ApiResult<PaginatedResult<CardResponse>>> Handle(MyCardsQuery request, CancellationToken cancellationToken)
    {
        var cardsQueryable = _dbContext.Cards.Where(c => c.UserId == _currentUserProvider.UserId);

        cardsQueryable = Filter(cardsQueryable, request.FilterBy, request.SearchTerm!);
        cardsQueryable = Sort(cardsQueryable, request.SortBy);
        var paginatedResult = await GetPaginatedCardResults(cardsQueryable, request);

        return ResponseMessage<PaginatedResult<CardResponse>>.Success(paginatedResult, "success");
    }
}