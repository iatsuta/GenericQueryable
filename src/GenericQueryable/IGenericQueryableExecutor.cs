using System.Linq.Expressions;

using GenericQueryable.Fetching;

namespace GenericQueryable;

public interface IGenericQueryableExecutor
{
    Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression);

    IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> queryable, FetchRule<TSource> fetchRule)
        where TSource : class;
}