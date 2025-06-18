using System.Linq.Expressions;

namespace GenericQueryable;

public interface IGenericQueryableExecutor
{
    object Execute(LambdaExpression callExpression);
}
