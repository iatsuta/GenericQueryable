using GenericQueryable.Fetching;
using GenericQueryable.Services;

namespace GenericQueryable.DependencyInjection;

public interface IGenericQueryableSetup
{
	IGenericQueryableSetup SetFetchService<TFetchService>()
		where TFetchService : IFetchService;

	IGenericQueryableSetup SetTargetMethodExtractor<TTargetMethodExtractor>()
		where TTargetMethodExtractor : ITargetMethodExtractor;
}