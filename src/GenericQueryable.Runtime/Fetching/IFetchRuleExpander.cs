namespace GenericQueryable.Fetching;

public interface IFetchRuleExpander
{
    PropertyFetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule);
}