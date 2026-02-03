namespace GenericQueryable.Fetching;

public interface IFetchService
{
	IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
		where TSource : class;
}