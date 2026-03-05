using System.Collections.Concurrent;

using CommonFramework;

namespace GenericQueryable.Fetching;

public class UntypedFetchExpander : IFetchRuleExpander
{
    private readonly ConcurrentDictionary<Type, object> cache = [];

    public PropertyFetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        if (fetchRule is UntypedFetchRule<TSource> untypedFetchRule)
        {
            return this.cache
                .GetOrAddAs(typeof(TSource), _ => new ConcurrentDictionary<UntypedFetchRule<TSource>, PropertyFetchRule<TSource>>())
                .GetOrAdd(untypedFetchRule, _ =>
                {
                    var fetchPath = LambdaExpressionPath.Create(typeof(TSource), untypedFetchRule.Path.Split('.'));

                    return new PropertyFetchRule<TSource>([fetchPath]);
                });
        }

        return null;
    }
}