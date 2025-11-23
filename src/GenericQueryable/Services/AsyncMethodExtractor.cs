using System.Reflection;

using CommonFramework;

namespace GenericQueryable.Services;

public abstract class AsyncMethodExtractor(IReadOnlyList<Type> extensionsTypes) : TargetMethodExtractor(extensionsTypes)
{
	protected override string GetTargetMethodName(MethodInfo baseMethod)
	{
		return base.GetTargetMethodName(baseMethod).Skip("Generic", true);
	}
}