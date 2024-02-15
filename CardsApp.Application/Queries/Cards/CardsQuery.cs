using CardsApp.Application.Interfaces;
using CardsApp.Application.Services;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Entities;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CardsApp.Application.Queries.Cards;

public class CardsQuery: BasePaginatedItemsQuery, IRequest<ApiResult<PaginatedResult<CardResponse>>>
{
   
}

public class CardsQueryHandler : BaseCardsQueryableBuilder, IRequestHandler<CardsQuery, ApiResult<PaginatedResult<CardResponse>>>
{
    private readonly CardEntityToResponseMapper _mapper;
    public CardsQueryHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext, CardEntityToResponseMapper mapper)
    :base(dbContext,currentUserProvider)
    {
        _mapper = mapper;
    }

    public async Task<ApiResult<PaginatedResult<CardResponse>>> Handle(CardsQuery request, CancellationToken cancellationToken)
    {
        var cardsQueryable = BuildQuery();
        cardsQueryable = Filter(cardsQueryable, request.FilterBy, request.SearchTerm!);
        cardsQueryable = Sort(cardsQueryable, request.SortBy);
        var paginatedResult = await GetPaginatedCardResults(cardsQueryable, request);

        return ResponseMessage<PaginatedResult<CardResponse>>.Success(paginatedResult, "success");
    }
    
    public IQueryable<Card> Filter(IQueryable<Card> cardsQueryable, CardFilterables? filterBy, string searchTerm)
    {
        return filterBy switch
        {
            CardFilterables.Name => cardsQueryable.Where(x => x.Name == searchTerm),
            CardFilterables.Color => cardsQueryable.Where(x => x.Color == searchTerm),
            CardFilterables.Status => cardsQueryable.Where(x => x.Status == searchTerm),
            CardFilterables.DateCreated => cardsQueryable.Where(x=>x.CreatedAt == DateTime.Parse(searchTerm!).ToUniversalTime()),
            _=>cardsQueryable
        };
    }
    
    public IQueryable<Card> Sort(IQueryable<Card> cardsQueryable, CardFilterables? sortBy)
    {
        return sortBy switch
        {
            CardFilterables.Name => cardsQueryable.OrderBy(x => x.Name),
            CardFilterables.Color => cardsQueryable.OrderBy(x => x.Color),
            CardFilterables.Status => cardsQueryable.OrderBy(x => x.Status),
            CardFilterables.DateCreated => cardsQueryable.OrderBy(x=>x.CreatedAt),
            _=>cardsQueryable.OrderBy(x=>x.CreatedAt)
        };
    }

    public async Task<PaginatedResult<CardResponse>> GetPaginatedCardResults(IQueryable<Card> cardsQueryable,
        BasePaginatedItemsQuery itemsQuery)
    {
        var totalCards = await cardsQueryable.CountAsync();
        cardsQueryable = cardsQueryable.Skip(itemsQuery.Index * itemsQuery.PageSize).Take(itemsQuery.PageSize);

        return new PaginatedResult<CardResponse>
        {
            Data = _mapper.MapToResponseList(cardsQueryable),
            Index = itemsQuery.Index,
            PageSize = itemsQuery.PageSize,
            Total = totalCards
        };
    }
}