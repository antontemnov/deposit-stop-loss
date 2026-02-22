using System.Reflection;

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

        // Command/Query handlers are registered automatically by FastEndpoints
        return services;
    }
}
