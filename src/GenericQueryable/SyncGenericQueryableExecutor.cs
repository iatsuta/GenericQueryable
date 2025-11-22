using CommonFramework;

using GenericQueryable.Fetching;

using System.Linq.Expressions;
using System.Reflection;

namespace GenericQueryable;

public class SyncGenericQueryableExecutor(Type mainType, Type enumerableType) : GenericQueryableExecutor
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

    public override async Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> callExpression)
    {
        return base.Execute<TResult>(callExpression);
    }

    public override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
    {
        return source;
    }

    public static SyncGenericQueryableExecutor Default { get; } = new(typeof(Queryable), typeof(Enumerable));
}