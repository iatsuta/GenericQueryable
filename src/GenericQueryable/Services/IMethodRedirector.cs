using System.Linq.Expressions;

namespace GenericQueryable.Services;

public interface IMethodRedirector
{
	Expression<Func<Task<TResult>>> Redirect<TResult>(Expression<Func<Task<TResult>>> callExpression);
}