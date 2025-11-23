using System.Linq.Expressions;
using System.Reflection;

namespace GenericQueryable.Services;

public class MethodRedirector(ITargetMethodExtractor targetMethodExtractor) : IMethodRedirector
{
	public Expression<Func<Task<TResult>>>? TryRedirect<TResult>(Expression<Func<Task<TResult>>> callExpression)
	{
		if (callExpression.Body is MethodCallExpression methodCallExpression)
		{
			var targetMethod = targetMethodExtractor.TryGetTargetMethod(methodCallExpression.Method);

			if (targetMethod != null)
			{
				var args = methodCallExpression.Arguments.Take(targetMethod.GetParameters().Length);

				var newCallExpression = this.CreateCallExpression(targetMethod, args);

				return Expression.Lambda<Func<Task<TResult>>>(newCallExpression);
			}
		}

		return null;
	}

	protected virtual Expression CreateCallExpression(MethodInfo targetMethod, IEnumerable<Expression> args)
	{
		return Expression.Call(targetMethod, args);
	}
}