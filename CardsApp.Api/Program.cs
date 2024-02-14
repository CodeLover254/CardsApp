using CardsApp.Api.Filters;
using CardsApp.Application;
using CardsApp.Application.Interfaces;
using CardsApp.Application.Services;
using CardsApp.Domain;
using CardsApp.Domain.Mappers.Cards;
using CardsApp.Domain.Settings;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//configure dependencies
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(c => c.Filters.Add<GlobalExceptionFilter>());
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddDbContext<CardAppDbContext>(c =>
{
    c.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddMediatRLibrary();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddSingleton<CardEntityToResponseMapper>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Name));
builder.Services.Configure<AppUserSettings>(builder.Configuration.GetSection(AppUserSettings.Name));
builder.Services.AddScoped<IAppSetupService, AppSetupService>();

var app = builder.Build();
//setup middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

//perform pre startup tasks
var appSetup = app.Services.GetService<IAppSetupService>();
await appSetup.MigrateDatabase();
await appSetup.SeedDefaultUsers();

//run application
app.Run();
