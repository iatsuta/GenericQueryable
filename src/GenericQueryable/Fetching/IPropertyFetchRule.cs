namespace GenericQueryable.Fetching;

public interface IPropertyFetchRule<TSource, out TLastProperty> : IPropertyFetchRule<TSource>;

public interface IPropertyFetchRule<TSource> : IFetchRule<TSource>
{
    IReadOnlyList<FetchPath> Paths { get; }
}