using System.Linq.Expressions;
using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Constants;
using CardsApp.Domain.Entities;

namespace CardsApp.Application.Services;

public class BaseCardsQueryableBuilder
{
    protected readonly CardAppDbContext DbContext;
    protected readonly ICurrentUserProvider CurrentUserProvider;
    
    public BaseCardsQueryableBuilder(CardAppDbContext dbContext, ICurrentUserProvider currentUserProvider)
    {
        DbContext = dbContext;
        CurrentUserProvider = currentUserProvider;
    }

    public IQueryable<Card> BuildQuery(Expression<Func<Card,bool>>? query=null)
    {
        var cardQueryable = DbContext.Cards.AsQueryable();
        if (CurrentUserProvider.UserRole!.Equals(UserRoles.Member))
        {
            cardQueryable = cardQueryable.Where(x => x.UserId == CurrentUserProvider.UserId);
        }

        if (query == null) return cardQueryable;
        
        return cardQueryable.Where(query);
    }
}