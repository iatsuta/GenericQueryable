namespace GenericQueryable.Fetching;

public abstract record FetchRuleHeader<TSource> : FetchRule<TSource>;

public record FetchRuleHeader<TSource, TValue>(TValue Value) : FetchRuleHeader<TSource>;