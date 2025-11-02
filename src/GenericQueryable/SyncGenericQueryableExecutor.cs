using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using GenericQueryable.Fetching;

namespace GenericQueryable;

public class SyncGenericQueryableExecutor(Type extensionsType) : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = extensionsType;

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

    protected override MethodInfo GetTargetMethod(MethodInfo baseMethod)
    {
        if (baseMethod.Name == nameof(GenericQueryableExtensions.GenericToListAsync))
        {
            return typeof(Enumerable).GetMethod(nameof(Enumerable.ToList))!.MakeGenericMethod(
                baseMethod.GetGenericArguments());
        }
        else
        {
            return base.GetTargetMethod(baseMethod);
        }
    }

    public override Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> callExpression)
    {
        var pureResult = base.Execute<TResult>(callExpression);

        return Task.FromResult(pureResult);
    }

    public override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
    {
        return source;
    }

    public static SyncGenericQueryableExecutor Default { get; } = new(typeof(Queryable));

}