namespace GenericQueryable.Fetching;

public abstract record FetchRuleHeaderInfo
{
    public abstract Type SourceType { get; }
}

public record FetchRuleHeaderInfo<TSource>(FetchRuleHeader<TSource> Header, PropertyFetchRule<TSource> Implementation) : FetchRuleHeaderInfo
{
    public override Type SourceType { get; } = typeof(TSource);
}