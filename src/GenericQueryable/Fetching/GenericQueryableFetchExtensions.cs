namespace GenericQueryable.Fetching;

public static class GenericQueryableFetchExtensions
{
    public static IQueryable<TSource> WithFetch<TSource>(this IQueryable<TSource> source, string fetchPath)
        where TSource : class
        => source.WithFetch(new UntypedFetchRule<TSource>(fetchPath));

    public static IQueryable<TSource> WithFetch<TSource>(this IQueryable<TSource> source,
        Func<PropertyFetchRule<TSource>, PropertyFetchRule<TSource>> fetchRule)
        where TSource : class
        => source.WithFetch(fetchRule(new PropertyFetchRule<TSource>([])));

    public static IQueryable<TSource> WithFetch<TSource>(this IQueryable<TSource> source, FetchRule<TSource> fetchRule)
        where TSource : class
        => source.Execute(executor => executor.ApplyFetch(source, fetchRule));
}