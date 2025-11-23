using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;

using GenericQueryable.Fetching;

namespace GenericQueryable.Services;

public class GenericQueryableExecutor(IEnumerable<IMethodRedirector> methodRedirectors, IFetchService fetchService) : IGenericQueryableExecutor
{
	private readonly ILambdaCompileCache lambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.None);

	public IFetchService FetchService { get; } = fetchService;

	public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression)
	{
		var redirectedExpressionRequest =

			from methodRedirector in methodRedirectors

			let result = methodRedirector.TryRedirect(expression)

			where result != null

			select result;

		var redirectedExpression = redirectedExpressionRequest.FirstOrDefault() ??
		                           throw new ArgumentOutOfRangeException(nameof(expression), "Expression can't be redirected");

		return await lambdaCompileCache.GetFunc(redirectedExpression).Invoke();
	}

	public static IGenericQueryableExecutor Sync { get; } =
		new GenericQueryableExecutor(
			[
				new SyncMethodRedirector(SyncTargetMethodExtractor.Queryable),
				new AsyncEnumerableMethodRedirector(new AsyncEnumerableMethodExtractor())
			],
			new IgnoreFetchService());
}