using System.Reflection;

namespace GenericQueryable.Services;

public interface ITargetMethodExtractor
{
	MethodInfo GetTargetMethod(MethodInfo baseMethod);
}