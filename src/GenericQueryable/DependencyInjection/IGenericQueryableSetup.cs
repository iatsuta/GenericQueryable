using GenericQueryable.Fetching;
using GenericQueryable.Services;

namespace GenericQueryable.DependencyInjection;

public interface IGenericQueryableSetup
{
	IGenericQueryableSetup SetFetchService<TFetchService>()
		where TFetchService : IFetchService;

    IGenericQueryableSetup AddFetchRuleExpander<TFetchRuleExpander>()
        where TFetchRuleExpander : IFetchRuleExpander;

    IGenericQueryableSetup AddFetchRule<TSource>(FetchRule<TSource> header, FetchRule<TSource> implementation);

    IGenericQueryableSetup SetTargetMethodExtractor<TTargetMethodExtractor>()
		where TTargetMethodExtractor : ITargetMethodExtractor;
}