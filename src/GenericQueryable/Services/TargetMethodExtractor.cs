using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;

namespace GenericQueryable.Services;

public abstract class TargetMethodExtractor : ITargetMethodExtractor
{
	private readonly IDictionaryCache<MethodInfo, MethodInfo> mappingMethodCache;

	protected TargetMethodExtractor() =>
		this.mappingMethodCache = new DictionaryCache<MethodInfo, MethodInfo>(this.GetInternalTargetMethod).WithLock();

	protected abstract IReadOnlyList<Type> ExtensionsTypes { get; }

	public MethodInfo GetTargetMethod(MethodInfo baseMethod)
	{
		return this.mappingMethodCache[baseMethod];
	}

	protected virtual string GetTargetMethodName(MethodInfo baseMethod) => baseMethod.Name.Skip("Generic", true);

	protected virtual int GetParameterCount(MethodInfo baseMethod)
	{
		return baseMethod.GetParameters().Length;
	}

	protected virtual IEnumerable<Type> GetTargetMethodParameterTypes(MethodInfo targetMethod)
	{
		return targetMethod.GetParameters().Select(p => p.ParameterType);
	}

	private MethodInfo GetInternalTargetMethod(MethodInfo baseMethod)
	{
		var targetMethodName = this.GetTargetMethodName(baseMethod);

		var genericArgs = baseMethod.GetGenericArguments();

		var parameterTypes = baseMethod.GetParameters().Take(this.GetParameterCount(baseMethod)).Select(p => p.ParameterType).ToArray();

		var request =

			from extensionsType in this.ExtensionsTypes

			from method in extensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)

			where method.Name == targetMethodName && method.GetGenericArguments().Length == genericArgs.Length

			let targetMethod = method.IsGenericMethodDefinition ? method.MakeGenericMethod(genericArgs) : method

			where this.GetTargetMethodParameterTypes(targetMethod).SequenceEqual(parameterTypes)

			select targetMethod;

		return request.Single();
	}
}