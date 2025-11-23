using System.Linq.Expressions;

using GenericQueryable.Fetching;

namespace GenericQueryable.Services;

public interface IGenericQueryableExecutor
{
	IFetchService FetchService { get; }

	Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression);
}