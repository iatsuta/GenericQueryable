using GenericQueryable.Fetching;
using GenericQueryable.Services;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.DependencyInjection;

public class GenericQueryableSetup : IGenericQueryableSetup
{
	private Type targetMethodExtractorType = typeof(SyncTargetMethodExtractor);

	private Type fetchServiceType = typeof(IgnoreFetchService);

	public void Initialize(IServiceCollection services)
	{
		services.AddSingleton<IGenericQueryableExecutor, GenericQueryableExecutor>();
		services.AddSingleton<IMethodRedirector, MethodRedirector>();

		services.AddSingleton(typeof(IFetchService), this.fetchServiceType);
		services.AddSingleton(typeof(ITargetMethodExtractor), this.targetMethodExtractorType);
	}

	public IGenericQueryableSetup SetFetchService<TFetchService>() where TFetchService : IFetchService
	{
		this.fetchServiceType = typeof(TFetchService);

		return this;
	}

	public IGenericQueryableSetup SetTargetMethodExtractor<TTargetMethodExtractor>() where TTargetMethodExtractor : ITargetMethodExtractor
	{
		this.targetMethodExtractorType = typeof(TTargetMethodExtractor);

		return this;
	}
}