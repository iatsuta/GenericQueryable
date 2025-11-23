using GenericQueryable.Fetching;

using System.Linq.Expressions;

namespace GenericQueryable;

public static class PropertyFetchRuleExtensions
{
    public static PropertyFetchRule<TSource, TNextProperty> ThenFetch<TSource, TLastProperty, TNextProperty>(this IPropertyFetchRule<TSource, TLastProperty> fetchRule,
        Expression<Func<TLastProperty, TNextProperty>> prop)
    {
        return fetchRule.ThenFetchInternal<TSource, TNextProperty>(prop);
    }

    public static PropertyFetchRule<TSource, TNextProperty> ThenFetch<TSource, TLastProperty, TNextProperty>(this IPropertyFetchRule<TSource, IEnumerable<TLastProperty>> fetchRule,
        Expression<Func<TLastProperty, TNextProperty>> prop)
    {
        return fetchRule.ThenFetchInternal<TSource, TNextProperty>(prop);
    }

    private static PropertyFetchRule<TSource, TNextProperty> ThenFetchInternal<TSource, TNextProperty>(this IPropertyFetchRule<TSource> fetchRule, LambdaExpression prop)
    {
        var prevPaths = fetchRule.Paths.SkipLast(1);

        var lastPath = fetchRule.Paths.Last();

        var newLastPath = new FetchPath(lastPath.Properties.Concat([prop]).ToList());

        return new PropertyFetchRule<TSource, TNextProperty>(prevPaths.Concat([newLastPath]).ToList());
    }
}