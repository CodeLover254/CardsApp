using System.Security.Claims;
using CardsApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CardsApp.Application.Services;

public class CurrentUserProvider: ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor!.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}