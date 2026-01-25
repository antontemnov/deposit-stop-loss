using System.Reflection;

using DepositStopLoss.Application.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace DepositStopLoss.Application;

/// <summary>
///     Dependency injection configuration for Application layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        // Register Application Services
        services.AddScoped<IDepositCalculator, DepositCalculator>();

        // Register Command/Query Handlers
        // TODO: Register handlers when they are created
        return services;
    }
}
