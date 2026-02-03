using System.Linq.Expressions;

namespace GenericQueryable.Services;

public interface IMethodRedirector
{
	Expression<Func<Task<TResult>>>? TryRedirect<TResult>(Expression<Func<Task<TResult>>> callExpression);
}