using CardsApp.Domain.Dto.Cards;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Entities;
using CardsApp.Domain.Enums;
using CardsApp.Domain.Mappers.Cards;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Application.Queries.Cards;

public class BaseCardsQueryHandler
{
    private readonly CardEntityToResponseMapper _mapper;

    public BaseCardsQueryHandler(CardEntityToResponseMapper mapper)
    {
        _mapper = mapper;
    }

    public IQueryable<Card> Filter(IQueryable<Card> cardsQueryable, CardFilterables? filterBy, string searchTerm)
    {
        return filterBy switch
        {
            CardFilterables.Name => cardsQueryable.Where(x => x.Name == searchTerm),
            CardFilterables.Color => cardsQueryable.Where(x => x.Color == searchTerm),
            CardFilterables.Status => cardsQueryable.Where(x => x.Status == Enum.Parse<CardStatus>(searchTerm!)),
            CardFilterables.DateCreated => cardsQueryable.Where(x=>x.CreatedAt == DateTime.Parse(searchTerm!)),
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