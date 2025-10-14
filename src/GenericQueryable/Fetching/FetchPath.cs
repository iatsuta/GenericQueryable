using System.Linq.Expressions;

namespace GenericQueryable.Fetching;

public record FetchPath(IReadOnlyList<LambdaExpression> Properties);