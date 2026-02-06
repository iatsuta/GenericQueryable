using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using GenericQueryable.Fetching;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.EntityFramework;

public class EfFetchService([FromKeyedServices(RootFetchRuleExpander.Key)] IFetchRuleExpander fetchRuleExpander) : FetchService(fetchRuleExpander)
{
    public override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
        where TSource : class
    {
        if (fetchRule is UntypedFetchRule<TSource> untypedFetchRule)
        {
            return source.Include(untypedFetchRule.Path);
        }
        else
        {
            return base.ApplyFetch(source, fetchRule);
        }
    }

    protected override IEnumerable<MethodInfo> GetFetchMethods<TSource>(LambdaExpressionPath fetchPath)
        where TSource : class
    {
        return fetchPath
            .Properties
            .ZipStrong(new LambdaExpression?[] { null }.Concat(fetchPath.Properties.SkipLast(1)), (prop, prevProp) => new { prop, prevProp })
            .Select(pair => GetFetchMethod<TSource>(pair.prop, pair.prevProp));
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