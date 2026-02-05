using GenericQueryable.Fetching;

using System.Linq.Expressions;

using CommonFramework;

namespace GenericQueryable;

public static class PropertyFetchRuleExtensions
{
	public static PropertyFetchRule<TSource> ToFetchRule<TSource>(this Func<PropertyFetchRule<TSource>, PropertyFetchRule<TSource>> buildFetchRule)
	{
		return buildFetchRule(FetchRule<TSource>.Empty);
	}

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

        var newLastPath = new LambdaExpressionPath(lastPath.Properties.Concat([prop]).ToList());

        return new PropertyFetchRule<TSource, TNextProperty>(prevPaths.Concat([newLastPath]).ToList());
    }
}