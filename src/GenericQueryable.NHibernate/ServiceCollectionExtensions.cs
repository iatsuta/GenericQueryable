using CommonFramework;

using GenericQueryable.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.NHibernate;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNHibernateGenericQueryable(this IServiceCollection services, Action<IGenericQueryableBuilder>? setupAction = null)
    {
        return services.AddGenericQueryable(v => v
            .SetFetchService<NHibFetchService>()
            .SetTargetMethodExtractor<NHibTargetMethodExtractor>()
            .Pipe(builder => setupAction?.Invoke(builder)));
    }
}