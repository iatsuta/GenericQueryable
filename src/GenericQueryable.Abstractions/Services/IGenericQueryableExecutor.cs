using System.Linq.Expressions;

using GenericQueryable.Fetching;

namespace GenericQueryable.Services;

public interface IGenericQueryableExecutor
{
	IFetchService FetchService { get; }

    TResult Execute<TResult>(Expression<Func<TResult>> callExpression);
}