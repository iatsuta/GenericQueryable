using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;

using GenericQueryable.Fetching;

namespace GenericQueryable.Services;

public class GenericQueryableExecutor(IEnumerable<IMethodRedirector> methodRedirectors, IFetchService fetchService) : IGenericQueryableExecutor
{
	private readonly ILambdaCompileCache lambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.None);

	public IFetchService FetchService { get; } = fetchService;

	public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> callExpression)
	{
		var redirectedExpressionRequest =

			from methodRedirector in methodRedirectors

			let result = methodRedirector.TryRedirect(callExpression)

			where result != null

			select result;

		var redirectedExpression = redirectedExpressionRequest.FirstOrDefault()
		                           ?? throw new ArgumentOutOfRangeException(nameof(callExpression), "Expression can't be redirected");

		return await lambdaCompileCache.GetFunc(redirectedExpression).Invoke();
	}

	public static IGenericQueryableExecutor Sync { get; } =

		new GenericQueryableExecutor([SyncMethodRedirector.Queryable, AsyncEnumerableMethodRedirector.Default], new IgnoreFetchService());
}