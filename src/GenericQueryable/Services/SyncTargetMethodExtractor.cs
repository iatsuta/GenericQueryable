using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public class SyncTargetMethodExtractor(IReadOnlyList<Type> extensionsTypes) : TargetMethodExtractor(extensionsTypes)
{
	protected override string GetTargetMethodName(MethodInfo baseMethod)
	{
		return base.GetTargetMethodName(baseMethod).SkipLast("Async", true);
	}

	protected override IEnumerable<Type> GetExpectedParameterTypes(MethodInfo baseMethod)
	{
		var parameterTypes = baseMethod.GetParameters().Select(p => p.ParameterType).ToList();

		if (parameterTypes.Last() != typeof(CancellationToken))
		{
			throw new InvalidOperationException(
				$"The last parameter of the method '{baseMethod.Name}' must be of type {nameof(CancellationToken)}.");
		}
		else
		{
			return parameterTypes.SkipLast(1);
		}
	}

	public static SyncTargetMethodExtractor Queryable { get; } = new([typeof(Queryable)]);
}