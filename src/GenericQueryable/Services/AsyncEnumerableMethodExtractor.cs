using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public class AsyncEnumerableMethodExtractor() : TargetMethodExtractor([typeof(AsyncEnumerable)])
{
	protected override IEnumerable<Type> GetTargetMethodParameterTypes(MethodInfo targetMethod)
	{
		var baseTypes = targetMethod.GetParameters().Select(p => p.ParameterType).ToList();

		var sourceType = baseTypes.First().GetGenericTypeImplementationArgument(typeof(IAsyncEnumerable<>))
		                 ?? throw new ArgumentOutOfRangeException(nameof(targetMethod), "Invalid binding method");

		return new[] { typeof(IQueryable<>).MakeGenericType(sourceType) }.Concat(baseTypes.Skip(1));
	}
}