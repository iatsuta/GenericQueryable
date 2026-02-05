using System.Linq.Expressions;

using CommonFramework;

namespace GenericQueryable.Fetching;

public abstract record FetchRule<TSource>
{
    public static FetchRule<TSource> Create(string path)
    {
        return new UntypedFetchRule<TSource>(path);
    }

    public static PropertyFetchRule<TSource, TProperty> Create<TProperty>(Expression<Func<TSource, TProperty>> prop)
    {
        return new PropertyFetchRule<TSource, TProperty>([new LambdaExpressionPath([prop])]);
    }

    public static PropertyFetchRule<TSource> Empty { get; } = new([]);
}