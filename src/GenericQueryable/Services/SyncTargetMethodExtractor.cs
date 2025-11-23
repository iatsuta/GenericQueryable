using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public class SyncTargetMethodExtractor(Type mainType, Type enumerableType) : TargetMethodExtractor
{
	private readonly IReadOnlySet<string> enumerableMethods =
		new[] { nameof(Enumerable.ToList), nameof(Enumerable.ToHashSet), nameof(Enumerable.ToDictionary) }.ToHashSet();

	protected override IReadOnlyList<Type> ExtensionsTypes { get; } = [mainType, enumerableType];

	protected override string GetTargetMethodName(MethodInfo baseMethod) => base.GetTargetMethodName(baseMethod).SkipLast("Async", true);

	protected override int GetParameterCount(MethodInfo baseMethod)
	{
		if (baseMethod.GetParameters().Last().ParameterType != typeof(CancellationToken))
		{
			throw new InvalidOperationException(
				$"The last parameter of the method '{baseMethod.Name}' must be of type {nameof(CancellationToken)}.");
		}
		else
		{
			return baseMethod.GetParameters().Length - 1;
		}
	}

	protected override IEnumerable<Type> GetTargetMethodParameterTypes(MethodInfo targetMethod)
	{
		if (enumerableMethods.Contains(targetMethod.Name))
		{
			var baseTypes = targetMethod.GetParameters().Select(p => p.ParameterType).ToList();

			var sourceType = baseTypes.First().GetGenericTypeImplementationArgument(typeof(IEnumerable<>))!;

			return new[] { typeof(IQueryable<>).MakeGenericType(sourceType) }.Concat(baseTypes.Skip(1));
		}
		else
		{
			return base.GetTargetMethodParameterTypes(targetMethod);
		}
	}
}