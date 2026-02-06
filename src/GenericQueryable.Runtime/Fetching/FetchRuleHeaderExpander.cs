using System.Collections.Concurrent;

using CommonFramework;

namespace GenericQueryable.Fetching;

public class FetchRuleHeaderExpander(IEnumerable<FetchRuleHeaderInfo> fetchRuleHeaderInfoList) : IFetchRuleExpander
{
    private readonly IReadOnlyDictionary<Type, IReadOnlyList<FetchRuleHeaderInfo>> headersDict =
        fetchRuleHeaderInfoList.GroupBy(v => v.SourceType).ToDictionary(g => g.Key, IReadOnlyList<FetchRuleHeaderInfo> (g) => g.ToList());

    private readonly ConcurrentDictionary<Type, object> cache = new();

    public PropertyFetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        if (fetchRule is FetchRuleHeader<TSource> fetchRuleHeader)
        {
            return cache.GetOrAdd(fetchRuleHeader.GetType(),
                    _ => headersDict
                        .GetValueOrDefault(typeof(TSource))
                        .EmptyIfNull()
                        .Cast<FetchRuleHeaderInfo<TSource>>()
                        .ToDictionary(info => info.Header, info => info.Implementation))

                .Pipe(innerCache => (IReadOnlyDictionary<FetchRuleHeader<TSource>, PropertyFetchRule<TSource>>)innerCache)

                .Pipe(innerCache => innerCache.GetValueOrDefault(fetchRuleHeader));
        }
        else
        {
            return null;
        }
    }
}