using CommonFramework.GenericRepository;

using GenericQueryable.IntegrationTests.Domain;
using GenericQueryable.IntegrationTests.Environment;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.IntegrationTests;

public abstract class MainTests(TestEnvironment testEnvironment) : IAsyncLifetime
{
    private readonly Guid testObjId = Guid.NewGuid();

    public async ValueTask InitializeAsync()
    {
        await testEnvironment.InitializeDatabase();

        var cancellationToken = TestContext.Current.CancellationToken;

        await using var scope = testEnvironment.RootServiceProvider.CreateAsyncScope();

        var serviceProvider = scope.ServiceProvider;

        var genericRepository = serviceProvider.GetRequiredService<IGenericRepository>();

        var fetchObj = new FetchObject();

        await genericRepository.SaveAsync(fetchObj, cancellationToken);
        await genericRepository.SaveAsync(new TestObject { Id = testObjId, FetchObject = fetchObj }, cancellationToken);
    }

    [Fact]
    public virtual async Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        await using var scope = testEnvironment.RootServiceProvider.CreateAsyncScope();

        var serviceProvider = scope.ServiceProvider;

        var queryableSource = serviceProvider.GetRequiredService<IQueryableSource>();

        var testSet = queryableSource.GetQueryable<TestObject>();

        // Act
        var result0 = await testSet
            .WithFetch(AppFetchRule.TestFetchRule)
            .GenericToArrayAsync(cancellationToken);

        var result1 = await testSet
            .WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
            .GenericToListAsync(cancellationToken);

        var result2 = await testSet
            .WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
            .GenericToHashSetAsync(cancellationToken);

        var result3 = await testSet
            .WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
            .GenericToDictionaryAsync(v => v.Id, cancellationToken);

        var result4 = await testSet
            .WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
            .GenericToDictionaryAsync(v => v.Id, v => v, cancellationToken);

        var result5 = await testSet
            //.WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
            .GenericAsAsyncEnumerable()
            .Take(100)
            .ToArrayAsync(cancellationToken);

        //Assert
        result0.Should().ContainSingle(testObj => testObj.Id == testObjId);
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}