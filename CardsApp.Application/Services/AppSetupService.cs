using CardsApp.Application.Interfaces;
using CardsApp.Domain;
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
    private readonly RoleManager<ApplicationUser> _roleManager;

    public AppSetupService(CardAppDbContext dbContext, 
        IOptions<AppUserSettings> options, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationUser> roleManager)
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
        var adminUser = _appUserSettings.Admin;
        var memberUser = _appUserSettings.Member;
        //todo complete
    }
}