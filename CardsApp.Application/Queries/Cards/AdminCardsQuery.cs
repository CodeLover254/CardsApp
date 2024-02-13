using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Mappers.Cards;
using MediatR;

namespace CardsApp.Application.Queries.Cards;

public class AdminCardsQuery: BasePaginatedItemsQuery, IRequest<ApiResult<PaginatedResult<CardResponse>>>
{
   
}

public class AdminCardsQueryHandler : BaseCardsQueryHandler,
    IRequestHandler<MyCardsQuery, ApiResult<PaginatedResult<CardResponse>>>
{
    private readonly CardAppDbContext _dbContext;

    public AdminCardsQueryHandler(CardAppDbContext dbContext,CardEntityToResponseMapper mapper) : base(mapper)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResult<PaginatedResult<CardResponse>>> Handle(MyCardsQuery request, CancellationToken cancellationToken)
    {
        var cardsQueryable = _dbContext.Cards.Where(x=>true);
        
        cardsQueryable = Filter(cardsQueryable, request.FilterBy, request.SearchTerm!);
        cardsQueryable = Sort(cardsQueryable, request.SortBy);
        var paginatedResult = await GetPaginatedCardResults(cardsQueryable, request);

        return ResponseMessage<PaginatedResult<CardResponse>>.Success(paginatedResult, "success");
    }
}