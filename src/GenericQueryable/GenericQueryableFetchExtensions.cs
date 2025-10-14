using GenericQueryable.Fetching;

using System.Linq.Expressions;

namespace GenericQueryable;

public static class GenericQueryableFetchExtensions
{
    public static IQueryable<TSource> WithFetch<TSource>(this IQueryable<TSource> source, string fetchPath)
    {
        return source.WithFetch(new UntypedFetchRule<TSource>(fetchPath));
    }

    public static IQueryable<TSource> WithFetch<TSource>(this IQueryable<TSource> source, IFetchRule<TSource> fetchRule)
    {
        Expression<Func<IQueryable<TSource>>> callExpression = () => source.WithFetch(fetchRule);

        return source.Provider.Execute<IQueryable<TSource>>(new GenericQueryableExecuteExpression(callExpression));
    }
}