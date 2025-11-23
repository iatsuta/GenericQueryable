using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.DependencyInjection;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddGenericQueryable(this IServiceCollection services, Action<IGenericQueryableSetup>? setupAction = null)
	{
		var setup = new GenericQueryableSetup();

		setupAction?.Invoke(setup);

		setup.Initialize(services);

		return services;
	}
}