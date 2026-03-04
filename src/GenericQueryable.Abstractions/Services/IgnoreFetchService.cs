using GenericQueryable.Fetching;

namespace GenericQueryable.Services;

public class IgnoreFetchService : IFetchService
{
	public IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule) where TSource : class => source;
}