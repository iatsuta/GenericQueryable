using System.Linq.Expressions;

namespace GenericQueryable.Services;

public class MethodRedirector(ITargetMethodExtractor targetMethodExtractor) : IMethodRedirector
{
	public Expression<Func<Task<TResult>>> Redirect<TResult>(Expression<Func<Task<TResult>>> callExpression)
	{
		if (callExpression.Body is MethodCallExpression methodCallExpression)
		{
			var targetMethod = targetMethodExtractor.GetTargetMethod(methodCallExpression.Method);

			var args = methodCallExpression.Arguments.Take(targetMethod.GetParameters().Length);

			var callExpr = this.PostCallExpression(Expression.Call(targetMethod, args));

			return Expression.Lambda<Func<Task<TResult>>>(callExpr);
		}
		else
		{
			throw new ArgumentOutOfRangeException(nameof(callExpression));
		}
	}

	protected virtual Expression PostCallExpression(Expression callExpression) => callExpression;
}