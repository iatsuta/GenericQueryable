using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public class AsyncEnumerableMethodRedirector(ITargetMethodExtractor targetMethodExtractor) : MethodRedirector(targetMethodExtractor)
{
	private static readonly MethodInfo ToAsyncEnumerableMethod = typeof(AsyncEnumerable).GetMethod(nameof(AsyncEnumerable.ToAsyncEnumerable))!;

	protected override Expression CreateCallExpression(MethodInfo targetMethod, IEnumerable<Expression> args)
	{
		var cachedArgs = args.ToList();

		var firstArg = cachedArgs.First();

		var sourceType = firstArg.Type.GetInterfaceImplementationArgument(typeof(IQueryable<>))
		                 ?? throw new ArgumentOutOfRangeException(nameof(args));

		var newFirstArg = Expression.Call(ToAsyncEnumerableMethod.MakeGenericMethod(sourceType), firstArg);

		var tail = cachedArgs.Skip(1);

		var newArgs = new[] { newFirstArg }.Concat(tail);

		return base.CreateCallExpression(targetMethod, newArgs).Pipe(WrapTask);
	}

	private static Expression WrapTask(Expression callExpression)
	{
		var asTaskMethod = callExpression.Type.GetMethod(nameof(ValueTask<>.AsTask))!;

		return Expression.Call(callExpression, asTaskMethod);
	}

	public static AsyncEnumerableMethodRedirector Default { get; } = new(AsyncEnumerableMethodExtractor.Default);
}