using CommonFramework;

namespace GenericQueryable.Fetching;

public interface IPropertyFetchRule<TSource, out TLastProperty> : IPropertyFetchRule<TSource>;

public interface IPropertyFetchRule<TSource>
{
    DeepEqualsCollection<LambdaExpressionPath> Paths { get; }
}