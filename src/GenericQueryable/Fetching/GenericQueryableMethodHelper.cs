using System.Reflection;

namespace GenericQueryable.Fetching;

public static class GenericQueryableMethodHelper
{
    public static readonly MethodInfo WithFetchRuleMethod =
        new Func<FetchRule<object>, IQueryable<object>>(Array.Empty<object>().AsQueryable().WithFetch).Method.GetGenericMethodDefinition();
}
