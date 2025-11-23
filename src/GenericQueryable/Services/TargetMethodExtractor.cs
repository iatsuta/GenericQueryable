using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;

namespace GenericQueryable.Services;

public class TargetMethodExtractor : ITargetMethodExtractor
{
	private readonly IReadOnlyList<Type> extensionsTypes;

	private readonly IDictionaryCache<MethodInfo, MethodInfo?> mappingMethodCache;

	protected TargetMethodExtractor(IReadOnlyList<Type> extensionsTypes)
	{
		this.extensionsTypes = extensionsTypes;

		this.mappingMethodCache = new DictionaryCache<MethodInfo, MethodInfo?>(this.GetInternalTargetMethod).WithLock();
	}

	public MethodInfo? TryGetTargetMethod(MethodInfo baseMethod)
	{
		return this.mappingMethodCache[baseMethod];
	}

	protected virtual string GetTargetMethodName(MethodInfo baseMethod)
	{
		return baseMethod.Name.Skip("Generic", true);
	}

	protected virtual IEnumerable<Type> GetExpectedParameterTypes(MethodInfo baseMethod)
	{
		return baseMethod.GetParameters().Select(p => p.ParameterType);
	}

	protected virtual IEnumerable<Type> GetTargetMethodParameterTypes(MethodInfo targetMethod)
	{
		return targetMethod.GetParameters().Select(p => p.ParameterType);
	}

	private MethodInfo? GetInternalTargetMethod(MethodInfo baseMethod)
	{
		var targetMethodName = this.GetTargetMethodName(baseMethod);

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
}