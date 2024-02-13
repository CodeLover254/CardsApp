using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CardsApp.Domain.Dto.Auth;
using CardsApp.Domain.Dto.Results;
using CardsApp.Domain.Entities;
using CardsApp.Domain.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CardsApp.Application.Commands.Auth;

public class UserLoginCommand: IRequest<ApiResult<UserLoginResponse?>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, ApiResult<UserLoginResponse?>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<UserLoginCommandHandler> _logger;
    private readonly JwtSettings _jwtSettings;
    private const string MessageForInvalidCredentials = "Invalid user or password";

    public UserLoginCommandHandler(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        ILogger<UserLoginCommandHandler> logger,IOptions<JwtSettings> jwtOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<ApiResult<UserLoginResponse?>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.UserName);
        if (user == null)
        {
            return ResponseMessage<UserLoginResponse?>.Error(null, MessageForInvalidCredentials);
        }

        var siginResult = await _signInManager.PasswordSignInAsync(user, request.Password,false,false);
        if (!siginResult.Succeeded)  return ResponseMessage<UserLoginResponse?>.Error(null, MessageForInvalidCredentials);

        var claims = await PopulateUserClaims(user);
        
        return ResponseMessage<UserLoginResponse?>.Success(new UserLoginResponse
        {
            AccessToken = GenerateAccessToken(claims),
            ExpiresIn = _jwtSettings.TokenExpirySeconds
        }, "success");
    }

    private async Task<List<Claim>> PopulateUserClaims(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
        };
        
        claims.AddRange(userClaims);
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role,userRole));
        }

        return claims;
    }

    private string GenerateAccessToken(List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, 
            expires: DateTime.Now.AddSeconds(_jwtSettings.TokenExpirySeconds), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}