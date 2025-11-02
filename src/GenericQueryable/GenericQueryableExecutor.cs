using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;
using CommonFramework.Maybe;

using GenericQueryable.Fetching;

namespace GenericQueryable;

public abstract class GenericQueryableExecutor : IGenericQueryableExecutor
{
    private readonly IDictionaryCache<MethodInfo, MethodInfo> mappingMethodCache;

    protected GenericQueryableExecutor() =>
        this.mappingMethodCache = new DictionaryCache<MethodInfo, MethodInfo>(this.GetTargetMethod).WithLock();

    protected abstract Type ExtensionsType { get; }

    protected virtual string GetTargetMethodName(MethodInfo baseMethod) => baseMethod.Name.Skip("Generic", true);

    protected virtual int GetParameterCount(MethodInfo baseMethod)
    {
        return baseMethod.GetParameters().Length;
    }

    protected virtual MethodInfo GetTargetMethod(MethodInfo baseMethod)
    {
        var targetMethodName = this.GetTargetMethodName(baseMethod);

        var genericArgs = baseMethod.GetGenericArguments();

        var parameterTypes = baseMethod.GetParameters().Take(this.GetParameterCount(baseMethod)).Select(p => p.ParameterType).ToArray();

        var request = 
            
            from method in this.ExtensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)

            where method.Name == targetMethodName && method.GetGenericArguments().Length == genericArgs.Length

            let targetMethod = method.IsGenericMethodDefinition ? method.MakeGenericMethod(genericArgs) : method

            where targetMethod.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes)

            select targetMethod;

        return request.Single();
    }

    public abstract Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression);

    public abstract IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
        where TSource : class;

    protected TResult Execute<TResult>(LambdaExpression callExpression)
    {
        if (callExpression.Body is MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Type.IsGenericTypeImplementation(typeof(Task<>)))
            {
                var args = methodCallExpression
                    .Arguments
                    .Take(this.GetParameterCount(methodCallExpression.Method))
                    .Select(arg => arg.GetMemberConstValue().GetValue()).ToArray();

                return this.mappingMethodCache[methodCallExpression.Method].Invoke<TResult>(null!, args!)!;
            }
        }

        throw new NotSupportedException();
    }
}