using System.Linq.Expressions;
using System.Reflection;

namespace GenericQueryable.Services;

public class SyncMethodRedirector(ITargetMethodExtractor targetMethodExtractor) : MethodRedirector(targetMethodExtractor)
{
	private static readonly MethodInfo TaskFromResultMethod = typeof(Task).GetMethod(nameof(Task.FromResult))!;

	protected override Expression PostCallExpression(Expression callExpression)
	{
		return Expression.Call(TaskFromResultMethod.MakeGenericMethod(callExpression.Type), callExpression);
	}
}