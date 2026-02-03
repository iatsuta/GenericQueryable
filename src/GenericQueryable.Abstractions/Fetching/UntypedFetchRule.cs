namespace GenericQueryable.Fetching;

public record UntypedFetchRule<TSource>(string Path) : FetchRule<TSource>;