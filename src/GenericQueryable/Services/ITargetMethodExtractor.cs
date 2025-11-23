using System.Reflection;

namespace GenericQueryable.Services;

public interface ITargetMethodExtractor
{
	MethodInfo? TryGetTargetMethod(MethodInfo baseMethod);
}