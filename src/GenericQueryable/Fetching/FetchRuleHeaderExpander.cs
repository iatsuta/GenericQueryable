using System.Collections.Concurrent;

using CommonFramework;

namespace GenericQueryable.Fetching;

public class FetchRuleHeaderExpander(IEnumerable<FetchRuleHeaderInfo> fetchRuleHeaderInfoList) : IFetchRuleExpander
{
    private readonly IReadOnlyDictionary<Type, IReadOnlyList<FetchRuleHeaderInfo>> headersDict =
        fetchRuleHeaderInfoList.GroupBy(v => v.SourceType).ToDictionary(g => g.Key, IReadOnlyList<FetchRuleHeaderInfo> (g) => g.ToList());

    private readonly ConcurrentDictionary<Type, object> cache = new();

    public FetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        return cache.GetOrAdd(typeof(TSource),
                _ => headersDict
                    .GetValueOrDefault(typeof(TSource))
                    .EmptyIfNull()
                    .Cast<FetchRuleHeaderInfo<TSource>>()
                    .ToDictionary(info => info.Header, info => info.Implementation))

            .Pipe(innerCache => (IReadOnlyDictionary<FetchRule<TSource>, FetchRule<TSource>>)innerCache)

            .Pipe(innerCache => innerCache.GetValueOrDefault(fetchRule));
    }
}