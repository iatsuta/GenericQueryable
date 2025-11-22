using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using GenericQueryable.Fetching;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GenericQueryable.EntityFramework;

public class EfGenericQueryableExecutor : GenericQueryableExecutor
{
    protected override IReadOnlyList<Type> ExtensionsTypes { get; } = [typeof(EntityFrameworkQueryableExtensions)];

    public override Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression)
    {
        return base.Execute<Task<TResult>>(expression);
    }

    public override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
    {
        return fetchRule switch
        {
            UntypedFetchRule<TSource> untypedFetchRule => source.Include(untypedFetchRule.Path),

            PropertyFetchRule<TSource> propertyFetchRule => this.ApplyFetch(source, propertyFetchRule),

            _ => throw new ArgumentOutOfRangeException(nameof(fetchRule))
        };
    }

    protected IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, PropertyFetchRule<TSource> fetchRule)
        where TSource : class
    {
        return fetchRule.Paths.Aggregate(source, this.ApplyFetch);
    }

    private IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchPath fetchPath)
        where TSource : class
    {
        return fetchPath
            .Properties
            .ZipStrong(new LambdaExpression?[] { null }.Concat(fetchPath.Properties.SkipLast(1)), (prop, prevProp) => new { prop, prevProp })
            .Aggregate(source, (q, pair) => this.ApplyFetch(q, pair.prop, pair.prevProp));
    }

    private IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, LambdaExpression prop, LambdaExpression? prevProp)
        where TSource : class
    {
        return this.GetFetchMethod<TSource>(prop, prevProp).Invoke<IQueryable<TSource>>(this, source, prop);
    }


    private MethodInfo GetFetchMethod<TSource>(LambdaExpression prop, LambdaExpression? prevProp)
        where TSource : class
    {
        if (prevProp == null)
        {
            return new Func<IQueryable<TSource>, Expression<Func<TSource, Ignore>>, IIncludableQueryable<TSource, Ignore>>(this.ApplyFetch)
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
                        this.ApplyThenFetch)
                    .CreateGenericMethod(typeof(TSource), prevElementType, nextPropertyType);
            }
            else
            {
                return new Func<IIncludableQueryable<TSource, Ignore>, Expression<Func<Ignore, Ignore>>, IIncludableQueryable<TSource, Ignore>>(
                        this.ApplyThenFetch)
                    .CreateGenericMethod(typeof(TSource), prevElementType, nextPropertyType);
            }
        }
    }

    private IIncludableQueryable<TSource, TProperty> ApplyFetch<TSource, TProperty>(IQueryable<TSource> source, Expression<Func<TSource, TProperty>> prop)
        where TSource : class
    {
        return source.Include(prop);
    }

    private IIncludableQueryable<TSource, TNextProperty> ApplyThenFetch<TSource, TPrevProperty, TNextProperty>(
        IIncludableQueryable<TSource, IEnumerable<TPrevProperty>> source, Expression<Func<TPrevProperty, TNextProperty>> prop)
        where TSource : class
    {
        return source.ThenInclude(prop);
    }

    private IIncludableQueryable<TSource, TNextProperty> ApplyThenFetch<TSource, TPrevProperty, TNextProperty>(
        IIncludableQueryable<TSource, TPrevProperty> source, Expression<Func<TPrevProperty, TNextProperty>> prop)
        where TSource : class
    {
        return source.ThenInclude(prop);
    }
}