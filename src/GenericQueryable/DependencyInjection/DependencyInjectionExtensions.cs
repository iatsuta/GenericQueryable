using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddGenericQueryable(this IServiceCollection services, Action<IGenericQueryableBuilder>? setupAction = null) =>
        services.Initialize<GenericQueryableBuilder>(setupAction);
}