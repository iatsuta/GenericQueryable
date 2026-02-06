using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.Fetching;

public abstract class FetchService([FromKeyedServices(RootFetchRuleExpander.Key)] IFetchRuleExpander fetchRuleExpander) : IFetchService
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>> rootCache = [];

    public virtual IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
        where TSource : class => this.GetApplyFetchFunc(fetchRule).Invoke(source);

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
            var nextApplyFunc = this.GetApplyFetchExpression<TSource>(path);

            return ExpressionEvaluateHelper.InlineEvaluate<Func<IQueryable<TSource>, IQueryable<TSource>>>(ee =>

                q => ee.Evaluate(nextApplyFunc, ee.Evaluate(state, q)));
        });
    }

    private Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> GetApplyFetchExpression<TSource>(LambdaExpressionPath fetchPath)
        where TSource : class
    {
        LambdaExpression startState = ExpressionHelper.GetIdentity<IQueryable<TSource>>();

        var resultBody = this
            .GetFetchMethods<TSource>(fetchPath).ZipStrong(fetchPath.Properties, (method, prop) => new { method, prop })
            .Aggregate(startState.Body, (state, pair) => Expression.Call(pair.method, state, pair.prop));

        return Expression.Lambda<Func<IQueryable<TSource>, IQueryable<TSource>>>(resultBody, startState.Parameters);
    }

    protected abstract IEnumerable<MethodInfo> GetFetchMethods<TSource>(LambdaExpressionPath fetchPath)
        where TSource : class;
}