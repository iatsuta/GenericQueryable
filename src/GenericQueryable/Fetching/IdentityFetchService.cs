namespace GenericQueryable.Fetching;

public class IdentityFetchService : IFetchService
{
	public IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule) where TSource : class => source;
}