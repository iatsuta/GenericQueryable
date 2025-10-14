using System.Linq.Expressions;

namespace GenericQueryable;

public static class GenericQueryableExtensions
{
    public static Task<List<TSource>> GenericToListAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericToListAsync(cancellationToken));

    public static Task<TSource> GenericSingleAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericSingleAsync(cancellationToken));

    public static Task<TSource> GenericSingleAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericSingleAsync(filter, cancellationToken));

    public static Task<TSource?> GenericSingleOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericSingleOrDefaultAsync(cancellationToken));

    public static Task<TSource?> GenericSingleOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericSingleOrDefaultAsync(filter, cancellationToken));

    public static Task<TSource> GenericFirstAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericFirstAsync(cancellationToken));

    public static Task<TSource?> GenericFirstOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericFirstOrDefaultAsync(cancellationToken));

    public static Task<int> GenericCountAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericCountAsync(cancellationToken));

    public static Task<bool> GenericAnyAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericAnyAsync(cancellationToken));

    public static Task<bool> GenericAnyAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericAnyAsync(filter, cancellationToken));

    public static Task<TSource?> GenericFirstOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericFirstOrDefaultAsync(filter, cancellationToken));

    public static Task<decimal?> GenericSumAsync(
        this IQueryable<decimal?> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericSumAsync(cancellationToken));

    public static Task<decimal?> GenericSumAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, decimal?>> selector,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.Execute(() => source.GenericSumAsync(selector, cancellationToken));

    private static Task<TResult> Execute<TSource, TResult>(
        this IQueryable<TSource> source,
        Expression<Func<Task<TResult>>> callExpression) =>
        source.Provider.Execute<Task<TResult>>(new GenericQueryableExecuteExpression(callExpression));
}
