namespace GenericQueryable.Fetching;

public abstract record FetchRuleHeader<TSource> : FetchRule<TSource>
{
    public static FetchRuleHeader<TSource, TValue> Create<TValue>(TValue value) => new (value);
}

public record FetchRuleHeader<TSource, TValue>(TValue Value) : FetchRuleHeader<TSource>;