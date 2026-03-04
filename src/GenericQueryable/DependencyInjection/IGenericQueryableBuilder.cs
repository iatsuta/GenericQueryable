using GenericQueryable.Fetching;
using GenericQueryable.Services;

namespace GenericQueryable.DependencyInjection;

public interface IGenericQueryableBuilder
{
	IGenericQueryableBuilder SetFetchService<TFetchService>()
		where TFetchService : IFetchService;

    IGenericQueryableBuilder AddFetchRuleExpander<TFetchRuleExpander>()
        where TFetchRuleExpander : IFetchRuleExpander;

    IGenericQueryableBuilder AddFetchRule<TSource>(FetchRuleHeader<TSource> header, PropertyFetchRule<TSource> implementation);

    IGenericQueryableBuilder SetTargetMethodExtractor<TTargetMethodExtractor>()
		where TTargetMethodExtractor : ITargetMethodExtractor;
}