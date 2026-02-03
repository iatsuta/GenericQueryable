using GenericQueryable.Services;

using System.Linq.Expressions;

namespace GenericQueryable;

public static class GenericQueryableExtensions
{
	extension<TSource>(IQueryable<TSource> source)
	{
		public Task<TSource[]> GenericToArrayAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericToArrayAsync(cancellationToken));

		public Task<List<TSource>> GenericToListAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericToListAsync(cancellationToken));

		public Task<HashSet<TSource>> GenericToHashSetAsync(CancellationToken cancellationToken = default) =>
			source.GenericToHashSetAsync(null, cancellationToken);

		public Task<HashSet<TSource>> GenericToHashSetAsync(IEqualityComparer<TSource>? comparer,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericToHashSetAsync(comparer, cancellationToken));

		public Task<Dictionary<TKey, TSource>> GenericToDictionaryAsync<TKey>(Func<TSource, TKey> keySelector,
			CancellationToken cancellationToken = default)
			where TKey : notnull =>
			source.GenericToDictionaryAsync(keySelector, null, cancellationToken);

		public Task<Dictionary<TKey, TSource>> GenericToDictionaryAsync<TKey>(Func<TSource, TKey> keySelector,
			IEqualityComparer<TKey>? comparer,
			CancellationToken cancellationToken = default)
			where TKey : notnull =>
			source.GenericToDictionaryAsync(keySelector, v => v, cancellationToken);

		public Task<Dictionary<TKey, TElement>> GenericToDictionaryAsync<TKey, TElement>(Func<TSource, TKey> keySelector,
			Func<TSource, TElement> elementSelector,
			CancellationToken cancellationToken = default)
			where TKey : notnull =>
			source.GenericToDictionaryAsync(keySelector, elementSelector, null, cancellationToken);

		public Task<Dictionary<TKey, TElement>> GenericToDictionaryAsync<TKey, TElement>(Func<TSource, TKey> keySelector,
			Func<TSource, TElement> elementSelector,
			IEqualityComparer<TKey>? comparer,
			CancellationToken cancellationToken = default)
			where TKey : notnull =>
			source.ExecuteAsync(() => source.GenericToDictionaryAsync(keySelector, elementSelector, comparer, cancellationToken));

		public Task<TSource> GenericSingleAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericSingleAsync(cancellationToken));

		public Task<TSource> GenericSingleAsync(Expression<Func<TSource, bool>> filter,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericSingleAsync(filter, cancellationToken));

		public Task<TSource?> GenericSingleOrDefaultAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericSingleOrDefaultAsync(cancellationToken));

		public Task<TSource?> GenericSingleOrDefaultAsync(Expression<Func<TSource, bool>> filter,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericSingleOrDefaultAsync(filter, cancellationToken));

		public Task<TSource> GenericFirstAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericFirstAsync(cancellationToken));

		public Task<TSource?> GenericFirstOrDefaultAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericFirstOrDefaultAsync(cancellationToken));

		public Task<int> GenericCountAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericCountAsync(cancellationToken));

		public Task<bool> GenericAllAsync(Expression<Func<TSource, bool>> filter,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericAllAsync(filter, cancellationToken));

		public Task<bool> GenericAnyAsync(CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericAnyAsync(cancellationToken));

		public Task<bool> GenericAnyAsync(Expression<Func<TSource, bool>> filter,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericAnyAsync(filter, cancellationToken));

		public Task<TSource?> GenericFirstOrDefaultAsync(Expression<Func<TSource, bool>> filter,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericFirstOrDefaultAsync(filter, cancellationToken));

		public Task<decimal?> GenericSumAsync(Expression<Func<TSource, decimal?>> selector,
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericSumAsync(selector, cancellationToken));

		public Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> callExpression) =>
			source.Execute(executor => executor.ExecuteAsync(callExpression));

		public TResult Execute<TResult>(Func<IGenericQueryableExecutor, TResult> execute) =>
			execute((source.Provider as IGenericQueryProvider)?.Executor ?? GenericQueryableExecutor.Sync);
	}

	extension(IQueryable<decimal?> source)
	{
		public Task<decimal?> GenericSumAsync(
			CancellationToken cancellationToken = default) =>
			source.ExecuteAsync(() => source.GenericSumAsync(cancellationToken));
	}
}