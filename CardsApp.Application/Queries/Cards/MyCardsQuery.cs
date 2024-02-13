using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Application.Queries.Cards;

public class MyCardsQuery: BasePaginatedItemsQuery, IRequest<ApiResult<PaginatedResult<CardResponse>>>
{
    public CardFilterables? FilterBy { get; set; }
    public string? SearchTerm { get; set; }
    public CardFilterables? SortBy { get; set; }
}

public class MyCardsQueryHandler : IRequestHandler<MyCardsQuery, ApiResult<PaginatedResult<CardResponse>>>
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly CardAppDbContext _dbContext;
    private readonly CardEntityToResponseMapper _mapper;

    public MyCardsQueryHandler(ICurrentUserProvider currentUserProvider, CardAppDbContext dbContext, CardEntityToResponseMapper mapper)
    {
        _currentUserProvider = currentUserProvider;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResult<PaginatedResult<CardResponse>>> Handle(MyCardsQuery request, CancellationToken cancellationToken)
    {
        var cardsQueryable = _dbContext.Cards.Where(c => c.UserId == _currentUserProvider.UserId);
        
        if (request.FilterBy != null)
        {
            cardsQueryable = request.FilterBy switch
            {
                CardFilterables.Name => cardsQueryable.Where(x => x.Name == request.SearchTerm),
                CardFilterables.Color => cardsQueryable.Where(x => x.Color == request.SearchTerm),
                CardFilterables.Status => cardsQueryable.Where(x => x.Status == Enum.Parse<CardStatus>(request.SearchTerm!)),
                CardFilterables.DateCreated=>cardsQueryable.Where(x=>x.CreatedAt == DateTime.Parse(request.SearchTerm!)),
                _=>cardsQueryable
            };
        }

        if (request.SortBy != null)
        {
            cardsQueryable = request.SortBy switch
            {
                CardFilterables.Name => cardsQueryable.OrderBy(x => x.Name),
                CardFilterables.Color => cardsQueryable.OrderBy(x => x.Color),
                CardFilterables.Status => cardsQueryable.OrderBy(x => x.Status),
                CardFilterables.DateCreated=>cardsQueryable.OrderBy(x=>x.CreatedAt),
                _=>cardsQueryable
            };
        }

        var totalCards = await cardsQueryable.CountAsync(cancellationToken);
        cardsQueryable = cardsQueryable.Skip(request.Index * request.PageSize).Take(request.PageSize);

        var paginatedResult = new PaginatedResult<CardResponse>
        {
            Data = _mapper.MapToResponseList(cardsQueryable),
            Index = request.Index,
            PageSize = request.PageSize,
            Total = totalCards
        };

        return ResponseMessage<PaginatedResult<CardResponse>>.Success(paginatedResult, "success");
    }
}