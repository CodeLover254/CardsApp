using CardsApp.Application.Interfaces;
using CardsApp.Domain;
using CardsApp.Domain.Constants;
using CardsApp.Domain.Entities;
using CardsApp.Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CardsApp.Application.Services;

public class AppSetupService: IAppSetupService
{
    private readonly CardAppDbContext _dbContext;
    private readonly AppUserSettings _appUserSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AppSetupService(CardAppDbContext dbContext, 
        IOptions<AppUserSettings> options, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _appUserSettings = options.Value;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task MigrateDatabase()
    {
        await _dbContext.Database.MigrateAsync();
    }

    public async Task SeedDefaultUsers()
    {
        await CreateRoles();
        
        var admin = _appUserSettings.Admin;
        var member = _appUserSettings.Member;

        await CreateUser(admin, UserRoles.Admin);
        await CreateUser(member, UserRoles.Member);
    }

    private async Task CreateUser(AppUser appUser, string role)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == appUser.UserName);
        if (user == null)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = appUser.UserName,
                Email = appUser.UserName,
            }; 
            var result = await _userManager.CreateAsync(applicationUser, appUser.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, role);
            }
        }
    }

    private async Task CreateRoles()
    {
        string[] roles = [UserRoles.Admin, UserRoles.Member];
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}