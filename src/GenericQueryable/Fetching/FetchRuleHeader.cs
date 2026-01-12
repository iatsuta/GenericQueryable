namespace GenericQueryable.Fetching;

public record FetchRuleHeader<TSource>(string Name) : FetchRule<TSource>;