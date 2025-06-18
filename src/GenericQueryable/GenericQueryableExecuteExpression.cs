using System.Linq.Expressions;

namespace GenericQueryable;

public class GenericQueryableExecuteExpression(LambdaExpression callExpression) : Expression
{
    public LambdaExpression CallExpression { get; } = callExpression;
}
