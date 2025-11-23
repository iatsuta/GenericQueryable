using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;

using GenericQueryable.Fetching;

namespace GenericQueryable.Services;

public class GenericQueryableExecutor(IMethodRedirector methodRedirector, IFetchService fetchService) : IGenericQueryableExecutor
{
	private readonly ILambdaCompileCache lambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.None);

	public IFetchService FetchService { get; } = fetchService;

	public async Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression)
	{
		var redirectedExpression = methodRedirector.Redirect(expression);

		return await lambdaCompileCache.GetFunc(redirectedExpression).Invoke();
	}

	public static IGenericQueryableExecutor Sync { get; } =
		new GenericQueryableExecutor(
			new SyncMethodRedirector(
				new SyncTargetMethodExtractor(typeof(Queryable), typeof(Enumerable))),
			new IdentityFetchService());
}