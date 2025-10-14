namespace GenericQueryable.Fetching;

public record PropertyFetchRule<TSource>(IReadOnlyList<FetchPath> Paths) : FetchRule<TSource>, IPropertyFetchRule<TSource>;

public record PropertyFetchRule<TSource, TLastProperty>(IReadOnlyList<FetchPath> Paths) : PropertyFetchRule<TSource>(Paths), IPropertyFetchRule<TSource, TLastProperty>;