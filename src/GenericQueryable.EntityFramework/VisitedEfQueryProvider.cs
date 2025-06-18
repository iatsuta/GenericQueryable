using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GenericQueryable.EntityFramework;

public class VisitedEfQueryProvider(IQueryCompiler queryCompiler, IGenericQueryableExecutor genericQueryableExecutor) : EntityQueryProvider(queryCompiler)
{
    public override TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return (TResult)genericQueryableExecutor.Execute(genericQueryableExecuteExpression.CallExpression);
        }
        else
        {
            return base.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }

    public override TResult Execute<TResult>(Expression expression)
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return (TResult)genericQueryableExecutor.Execute(genericQueryableExecuteExpression.CallExpression);
        }
        else
        {
            return base.Execute<TResult>(expression);
        }
    }

    public override object Execute(Expression expression)
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return genericQueryableExecutor.Execute(genericQueryableExecuteExpression.CallExpression);
        }
        else
        {
            return base.Execute(expression);
        }
    }
}