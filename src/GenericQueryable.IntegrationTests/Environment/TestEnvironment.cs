using CommonFramework;
using CommonFramework.DependencyInjection;

using GenericQueryable.DependencyInjection;
using GenericQueryable.Fetching;
using GenericQueryable.IntegrationTests.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.IntegrationTests.Environment;

public abstract class TestEnvironment
{
    public IServiceProvider RootServiceProvider => field ??= BuildServiceProvider();

    protected IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .Pipe(this.InitializeServices)
            .AddValidator<DuplicateServiceUsageValidator>()
            .Validate()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }

    protected void SetupGenericQueryable(IGenericQueryableBuilder builder)
    {
        builder
            .AddFetchRuleExpander<AppFetchRuleExpander>()
            .AddFetchRule(AppFetchRule.TestFetchRule, FetchRule<TestObject>.Create(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject));
    }

    protected abstract IServiceCollection InitializeServices(IServiceCollection services);

    public abstract Task InitializeDatabase();
}