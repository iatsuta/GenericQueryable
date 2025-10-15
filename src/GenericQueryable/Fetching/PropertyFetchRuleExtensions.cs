using System.Linq.Expressions;

namespace GenericQueryable.Fetching;

public static class PropertyFetchRuleExtensions
{
    public static PropertyFetchRule<TSource, TNextProperty> Fetch<TSource, TLastProperty, TNextProperty>(
        this PropertyFetchRule<TSource, TLastProperty> fetchRule, Expression<Func<TSource, TNextProperty>> path)
    {
        return new PropertyFetchRule<TSource, TNextProperty>(fetchRule.Paths.Concat([new FetchPath([path])]).ToList());
    }

    public static PropertyFetchRule<TSource, TNextProperty> FetchThen<TSource, TLastProperty, TNextProperty>(this IPropertyFetchRule<TSource, TLastProperty> fetchRule,
        Expression<Func<TLastProperty, TNextProperty>> prop)
    {
        return fetchRule.FetchThenInternal<TSource, TNextProperty>(prop);
    }

    public static PropertyFetchRule<TSource, TNextProperty> FetchThen<TSource, TLastProperty, TNextProperty>(this IPropertyFetchRule<TSource, IEnumerable<TLastProperty>> fetchRule,
        Expression<Func<TLastProperty, TNextProperty>> prop)
    {
        return fetchRule.FetchThenInternal<TSource, TNextProperty>(prop);
    }

    private static PropertyFetchRule<TSource, TNextProperty> FetchThenInternal<TSource, TNextProperty>(this IPropertyFetchRule<TSource> fetchRule, LambdaExpression prop)
    {
        var prevPaths = fetchRule.Paths.SkipLast(1);

        var lastPath = fetchRule.Paths.Last();

        var newLastPath = new FetchPath(lastPath.Properties.Concat([prop]).ToList());

        return new PropertyFetchRule<TSource, TNextProperty>(prevPaths.Concat([newLastPath]).ToList());
    }
}