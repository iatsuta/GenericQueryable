namespace GenericQueryable.Fetching;

public sealed record UntypedFetchRule<TSource>(string Path) : FetchRule<TSource>;