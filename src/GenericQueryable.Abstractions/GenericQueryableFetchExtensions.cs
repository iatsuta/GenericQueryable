using GenericQueryable.Fetching;

namespace GenericQueryable;

public static class GenericQueryableFetchExtensions
{
    extension<TSource>(IQueryable<TSource> source)
        where TSource : class
    {
        public IQueryable<TSource> WithFetch(string fetchPath) => source.WithFetch(new UntypedFetchRule<TSource>(fetchPath));

        public IQueryable<TSource> WithFetch(Func<PropertyFetchRule<TSource>, PropertyFetchRule<TSource>> buildFetchRule) =>
            source.WithFetch(buildFetchRule.ToFetchRule());

        public IQueryable<TSource> WithFetch(FetchRule<TSource> fetchRule) => source.Execute(executor => executor.FetchService.ApplyFetch(source, fetchRule));
    }
}