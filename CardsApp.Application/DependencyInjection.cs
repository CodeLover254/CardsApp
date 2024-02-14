using System.Reflection;
using CardsApp.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CardsApp.Application;

public static class DependencyInjection
{
    public static void AddMediatRLibrary(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}