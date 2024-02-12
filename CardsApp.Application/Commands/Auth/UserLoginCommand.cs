using CardsApp.Domain.Dto;
using CardsApp.Domain.Dto.Auth;
using CardsApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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
    private readonly RoleManager<ApplicationUser> _roleManager;
    private readonly ILogger<UserLoginCommandHandler> _logger;

    public UserLoginCommandHandler(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        RoleManager<ApplicationUser> roleManager, 
        ILogger<UserLoginCommandHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public Task<ApiResult<UserLoginResponse?>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}