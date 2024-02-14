using System.Reflection;
using CardsApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CardsApp.Domain;

public class CardAppDbContext: IdentityDbContext<ApplicationUser>
{
    public CardAppDbContext(DbContextOptions<CardAppDbContext> contextOptions): base(contextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public DbSet<Card> Cards { get; set; }
}