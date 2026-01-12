using GenericQueryable.Fetching;
using GenericQueryable.IntegrationTests.Domain;

namespace GenericQueryable.IntegrationTests;

public static class AppFetchRule
{
    public static FetchRule<TestObject> TestFetchRule { get; } = new FetchRuleHeader<TestObject>(nameof(TestFetchRule));
}