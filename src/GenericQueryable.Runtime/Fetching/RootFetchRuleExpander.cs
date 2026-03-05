using System.Collections.Concurrent;

using CommonFramework;

namespace GenericQueryable.Fetching;

public class RootFetchRuleExpander(IEnumerable<IFetchRuleExpander> expanders) : IFetchRuleExpander
{
    public const string Key = "Root";

    private readonly ConcurrentDictionary<Type, object> cache = [];

    public PropertyFetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        if (fetchRule is PropertyFetchRule<TSource> propertyFetchRule)
        {
            return propertyFetchRule;
        }
        else
        {
            return cache
                .GetOrAdd(fetchRule.GetType(), () => new ConcurrentDictionary<FetchRule<TSource>, PropertyFetchRule<TSource>?>())
                .Pipe(innerCache => (ConcurrentDictionary<FetchRule<TSource>, PropertyFetchRule<TSource>?>)innerCache)
                .GetOrAdd(fetchRule, () =>
                {
                    var request =

                        from expander in expanders

                        let expandedFetchRule = expander.TryExpand(fetchRule)

                        where expandedFetchRule != null

                        select expandedFetchRule;

                    return request.FirstOrDefault();
                });
        }
    }
}