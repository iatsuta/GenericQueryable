using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public class SyncMethodRedirector(ITargetMethodExtractor targetMethodExtractor) : MethodRedirector(targetMethodExtractor)
{
	private static readonly MethodInfo TaskFromResultMethod = typeof(Task).GetMethod(nameof(Task.FromResult))!;

	protected override Expression CreateCallExpression(MethodInfo targetMethod, IEnumerable<Expression> args)
	{
		return base.CreateCallExpression(targetMethod, args).Pipe(WrapToAsync);
	}

	private static Expression WrapToAsync(Expression callExpression)
	{
		return Expression.Call(TaskFromResultMethod.MakeGenericMethod(callExpression.Type), callExpression);
	}
}