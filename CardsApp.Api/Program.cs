using System.Text;
using System.Text.Json.Serialization;
using CardsApp.Api.Filters;
using CardsApp.Application;
using CardsApp.Application.Interfaces;
using CardsApp.Application.Services;
using CardsApp.Domain;
using CardsApp.Domain.Entities;
using CardsApp.Domain.Mappers.Cards;
using CardsApp.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//configure dependencies
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "CardsApp API", Version = "v1" });
    cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddControllers(c => c.Filters.Add<GlobalExceptionFilter>())
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));;
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<CardAppDbContext>(c =>
{
    c.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<CardAppDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddSingleton<CardEntityToResponseMapper>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Name));
builder.Services.Configure<AppUserSettings>(builder.Configuration.GetSection(AppUserSettings.Name));
builder.Services.AddScoped<IAppSetupService, AppSetupService>();

builder.Services.AddMediatRLibrary();

builder.Services.AddAuthorization();
var jwtSettings = builder.Configuration.GetSection(JwtSettings.Name).Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings!.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });


var app = builder.Build();
//setup middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//perform pre startup tasks
using var scope = app.Services.CreateScope();
var appSetup = scope.ServiceProvider.GetService<IAppSetupService>();
await appSetup.MigrateDatabase();
await appSetup.SeedDefaultUsers();

//run application
app.Run();
