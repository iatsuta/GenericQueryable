using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;
using GenericQueryable.Fetching;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.EntityFramework;

public class EfFetchService([FromKeyedServices(RootFetchRuleExpander.Key)] IFetchRuleExpander fetchRuleExpander) : IFetchService
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>> rootCache = [];

    public virtual IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
        where TSource : class
    {
        return fetchRule switch
        {
            UntypedFetchRule<TSource> untypedFetchRule => source.Include(untypedFetchRule.Path),

            _ => this.GetApplyFetchFunc(fetchRule).Invoke(source)
        };
    }

    private Func<IQueryable<TSource>, IQueryable<TSource>> GetApplyFetchFunc<TSource>(FetchRule<TSource> fetchRule)
        where TSource : class
    {
        return this.rootCache
            .GetOrAdd(typeof(TSource), _ => new ConcurrentDictionary<Type, object>())
            .GetOrAdd(fetchRule.GetType(), _ => new ConcurrentDictionary<FetchRule<TSource>, Func<IQueryable<TSource>, IQueryable<TSource>>>())
            .Pipe(v => (ConcurrentDictionary<FetchRule<TSource>, Func<IQueryable<TSource>, IQueryable<TSource>>>)v)
            .GetOrAdd(fetchRule, _ =>
            {
                var fetchExpr = this.GetApplyFetchExpression(fetchRuleExpander.Expand(fetchRule));

                return fetchExpr.Compile();
            });
    }

    private Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> GetApplyFetchExpression<TSource>(PropertyFetchRule<TSource> fetchRule)
        where TSource : class
    {
        var startState = ExpressionHelper.GetIdentity<IQueryable<TSource>>();

        return fetchRule.Paths.Aggregate(startState, (state, path) =>
        {
            var nextApplyFunc = GetApplyFetchExpression<TSource>(path);

            return ExpressionEvaluateHelper.InlineEvaluate<Func<IQueryable<TSource>, IQueryable<TSource>>>(ee =>

                q => ee.Evaluate(nextApplyFunc, ee.Evaluate(state, q)));
        });
    }

    private static Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> GetApplyFetchExpression<TSource>(LambdaExpressionPath fetchPath)
        where TSource : class
    {
        LambdaExpression startState = ExpressionHelper.GetIdentity<IQueryable<TSource>>();

        var resultBody = fetchPath
            .Properties
            .ZipStrong(new LambdaExpression?[] { null }.Concat(fetchPath.Properties.SkipLast(1)), (prop, prevProp) => new { prop, prevProp })
            .Aggregate(startState.Body, (state, pair) =>
            {
                var fetchMethod = GetFetchMethod<TSource>(pair.prop, pair.prevProp);

                return Expression.Call(fetchMethod, state, pair.prop);
            });

        return Expression.Lambda<Func<IQueryable<TSource>, IQueryable<TSource>>>(resultBody, startState.Parameters);
    }

    private static MethodInfo GetFetchMethod<TSource>(LambdaExpression prop, LambdaExpression? prevProp)
		where TSource : class
	{
		if (prevProp == null)
		{
			return new Func<IQueryable<TSource>, Expression<Func<TSource, Ignore>>, IIncludableQueryable<TSource, Ignore>>(EntityFrameworkQueryableExtensions.Include)
				.CreateGenericMethod(typeof(TSource), prop.Body.Type);
		}
		else
		{
			var prevElementType = prop.Parameters.Single().Type;

			var prevPropRealType = prevProp.ReturnType;

			var nextPropertyType = prop.Body.Type;

			if (prevPropRealType.IsGenericType && typeof(IEnumerable<>).MakeGenericType(prevElementType).IsAssignableFrom(prevPropRealType))
			{
				return new Func<IIncludableQueryable<TSource, IEnumerable<Ignore>>, Expression<Func<Ignore, Ignore>>, IIncludableQueryable<TSource, Ignore>>(
                        EntityFrameworkQueryableExtensions.ThenInclude)
					.CreateGenericMethod(typeof(TSource), prevElementType, nextPropertyType);
			}
			else
			{
				return new Func<IIncludableQueryable<TSource, Ignore>, Expression<Func<Ignore, Ignore>>, IIncludableQueryable<TSource, Ignore>>(
                        EntityFrameworkQueryableExtensions.ThenInclude)
					.CreateGenericMethod(typeof(TSource), prevElementType, nextPropertyType);
			}
		}
	}
}