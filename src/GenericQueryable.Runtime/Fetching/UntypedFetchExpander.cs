using System.Collections.Concurrent;

using CommonFramework;

namespace GenericQueryable.Fetching;

public class UntypedFetchExpander : IFetchRuleExpander
{
    private readonly ConcurrentDictionary<Type, object> cache = new();

    public PropertyFetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        if (fetchRule is UntypedFetchRule<TSource> untypedFetchRule)
        {
            return this.cache.GetOrAdd(typeof(TSource), _ => new ConcurrentDictionary<UntypedFetchRule<TSource>, PropertyFetchRule<TSource>>())
                .Pipe(innerCache => (ConcurrentDictionary<UntypedFetchRule<TSource>, PropertyFetchRule<TSource>>)innerCache)
                .Pipe(innerCache => innerCache.GetOrAdd(
                    untypedFetchRule,
                    _ =>
                    {
                        var fetchPath = LambdaExpressionPath.Create(typeof(TSource), untypedFetchRule.Path.Split('.'));

                        return new PropertyFetchRule<TSource>([fetchPath]);
                    }));
        }

        return null;
    }
}