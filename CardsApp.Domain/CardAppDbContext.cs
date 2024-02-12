using CardsApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CardsApp.Domain;

public class CardAppDbContext: IdentityDbContext<ApplicationUser>
{
    
}