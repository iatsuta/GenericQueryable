namespace GenericQueryable.Fetching;

public interface IFetchRuleExpander
{
    FetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule);
}