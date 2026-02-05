using System.Linq.Expressions;

using CommonFramework;

namespace GenericQueryable.Fetching;

public record PropertyFetchRule<TSource>(DeepEqualsCollection<LambdaExpressionPath> Paths) : FetchRule<TSource>, IPropertyFetchRule<TSource>
{
    public PropertyFetchRule(IEnumerable<LambdaExpressionPath> paths) :
        this(DeepEqualsCollection.Create(paths))
    {
    }

    public PropertyFetchRule<TSource, TNextProperty> Fetch<TNextProperty>(Expression<Func<TSource, TNextProperty>> path)
    {
        return new PropertyFetchRule<TSource, TNextProperty>(this.Paths.Concat([new LambdaExpressionPath([path])]));
    }
}

public record PropertyFetchRule<TSource, TLastProperty>(DeepEqualsCollection<LambdaExpressionPath> Paths)
    : PropertyFetchRule<TSource>(Paths), IPropertyFetchRule<TSource, TLastProperty>
{
    public PropertyFetchRule(IEnumerable<LambdaExpressionPath> paths) :
        this(DeepEqualsCollection.Create(paths))
    {
    }
}