namespace GenericQueryable.Fetching;

public interface IPropertyFetchRule<TSource, out TLastProperty> : IPropertyFetchRule<TSource>;

public interface IPropertyFetchRule<TSource>
{
    IReadOnlyList<FetchPath> Paths { get; }
}