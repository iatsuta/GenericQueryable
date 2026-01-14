using GenericQueryable.Fetching;
using GenericQueryable.IntegrationTests.Domain;

namespace GenericQueryable.IntegrationTests;

public static class AppFetchRule
{
    public static FetchRuleHeader<TestObject> TestFetchRule { get; } = new FetchRuleHeader<TestObject, string>(nameof(TestFetchRule));
}