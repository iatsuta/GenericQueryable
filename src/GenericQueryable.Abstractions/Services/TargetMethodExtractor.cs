using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public class TargetMethodExtractor(ImmutableList<Type> extensionsTypes) : ITargetMethodExtractor
{
	private readonly ConcurrentDictionary<MethodInfo, MethodInfo?> mappingMethodCache = [];

	public MethodInfo? TryGetTargetMethod(MethodInfo baseMethod)
	{
		return this.mappingMethodCache.GetOrAdd(baseMethod, _ =>
        {
            if (this.TryGetTargetMethodName(baseMethod) is { } targetMethodName)
            {
                var genericArgs = baseMethod.GetGenericArguments();

                var expectedParameterTypes = this.GetExpectedParameterTypes(baseMethod).ToList();

                var request =

                    from extensionsType in extensionsTypes

                    from method in extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)

                    where method.Name == targetMethodName && method.GetGenericArguments().Length == genericArgs.Length

                    let targetMethod = method.IsGenericMethodDefinition ? method.MakeGenericMethod(genericArgs) : method

                    where expectedParameterTypes.SequenceEqual(this.GetTargetMethodParameterTypes(targetMethod))

                    select targetMethod;

                return request.SingleOrDefault();
            }
            else
            {
                return null;
            }
        });
	}

	protected virtual string? TryGetTargetMethodName(MethodInfo baseMethod)
    {
        return baseMethod.Name.StartsWith("Generic") ? baseMethod.Name.Skip("Generic", true) : null;
    }

	protected virtual IEnumerable<Type> GetExpectedParameterTypes(MethodInfo baseMethod)
	{
		return baseMethod.GetParameters().Select(p => p.ParameterType);
	}

	protected virtual IEnumerable<Type> GetTargetMethodParameterTypes(MethodInfo targetMethod)
	{
		return targetMethod.GetParameters().Select(p => p.ParameterType);
	}
}