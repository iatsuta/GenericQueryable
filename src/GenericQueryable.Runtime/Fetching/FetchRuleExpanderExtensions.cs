namespace GenericQueryable.Fetching;

public static class FetchRuleExpanderExtensions
{
    public static PropertyFetchRule<TSource> Expand<TSource>(this IFetchRuleExpander fetchRuleExpander, FetchRule<TSource> fetchRule) =>
        fetchRuleExpander.TryExpand(fetchRule) ?? throw new ArgumentOutOfRangeException(nameof(fetchRule));
}