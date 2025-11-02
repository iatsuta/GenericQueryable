using System.Linq.Expressions;

namespace GenericQueryable;

public static class GenericQueryableExtensions
{
    public static Task<List<TSource>> GenericToListAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericToListAsync(cancellationToken));

    public static Task<TSource> GenericSingleAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericSingleAsync(cancellationToken));

    public static Task<TSource> GenericSingleAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericSingleAsync(filter, cancellationToken));

    public static Task<TSource?> GenericSingleOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericSingleOrDefaultAsync(cancellationToken));

    public static Task<TSource?> GenericSingleOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericSingleOrDefaultAsync(filter, cancellationToken));

    public static Task<TSource> GenericFirstAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericFirstAsync(cancellationToken));

    public static Task<TSource?> GenericFirstOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericFirstOrDefaultAsync(cancellationToken));

    public static Task<int> GenericCountAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericCountAsync(cancellationToken));

    public static Task<bool> GenericAnyAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericAnyAsync(cancellationToken));

    public static Task<bool> GenericAnyAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericAnyAsync(filter, cancellationToken));

    public static Task<TSource?> GenericFirstOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericFirstOrDefaultAsync(filter, cancellationToken));

    public static Task<decimal?> GenericSumAsync(
        this IQueryable<decimal?> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericSumAsync(cancellationToken));

    public static Task<decimal?> GenericSumAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, decimal?>> selector,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ExecuteAsync(() => source.GenericSumAsync(selector, cancellationToken));

    public static Task<TResult> ExecuteAsync<TSource, TResult>(
        this IQueryable<TSource> source,
        Expression<Func<Task<TResult>>> callExpression) =>
        source.Execute(executor => executor.ExecuteAsync(callExpression));

    public static TResult Execute<TSource, TResult>(
        this IQueryable<TSource> source,
        Func<IGenericQueryableExecutor, TResult> execute) =>
        execute((source.Provider as IGenericQueryProvider)?.Executor ?? SyncGenericQueryableExecutor.Default);
}