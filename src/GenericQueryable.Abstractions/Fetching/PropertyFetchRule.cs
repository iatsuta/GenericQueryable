using System.Linq.Expressions;

namespace GenericQueryable.Fetching;

public record PropertyFetchRule<TSource>(IReadOnlyList<FetchPath> Paths) : FetchRule<TSource>, IPropertyFetchRule<TSource>
{
    public PropertyFetchRule<TSource, TNextProperty> Fetch<TNextProperty>(Expression<Func<TSource, TNextProperty>> path)
    {
        return new PropertyFetchRule<TSource, TNextProperty>(this.Paths.Concat([new FetchPath([path])]).ToList());
    }
}

public record PropertyFetchRule<TSource, TLastProperty>(IReadOnlyList<FetchPath> Paths) : PropertyFetchRule<TSource>(Paths), IPropertyFetchRule<TSource, TLastProperty>;