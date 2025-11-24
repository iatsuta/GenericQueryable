using GenericQueryable.EntityFramework;
using GenericQueryable.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.IntegrationTests;

public class MainTests
{
	[Fact]
	public async Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked()
	{
		// Arrange
		var ct = CancellationToken.None;

		var sp = new ServiceCollection()
			.AddDbContext<TestDbContext>(optionsBuilder => optionsBuilder
					.UseSqlite("Data Source=test.db")
					.UseGenericQueryable(),
				contextLifetime: ServiceLifetime.Singleton,
				optionsLifetime: ServiceLifetime.Singleton)
			.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

		var dbContext = sp.GetRequiredService<TestDbContext>();

		await dbContext.Database.EnsureDeletedAsync(ct);
		await dbContext.Database.EnsureCreatedAsync(ct);

		var testSet = dbContext.Set<TestObject>();

		var fetchObj = new FetchObject();

		await dbContext.Set<FetchObject>().AddAsync(fetchObj, ct);

		var testObj = new TestObject { Id = Guid.NewGuid() };

		await testSet.AddAsync(testObj, ct);

		await dbContext.SaveChangesAsync(ct);

		// Act
		var result0 = await testSet
			.WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
			.GenericToArrayAsync(cancellationToken: ct);

		var result1 = await testSet
			.WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
			.GenericToListAsync(cancellationToken: ct);

		var result2 = await testSet
			.WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
			.GenericToHashSetAsync(cancellationToken: ct);

		var result3 = await testSet
			.WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
			.GenericToDictionaryAsync(v => v.Id, cancellationToken: ct);

		var result4 = await testSet
			.WithFetch(r => r.Fetch(v => v.DeepFetchObjects).ThenFetch(v => v.FetchObject))
			.GenericToDictionaryAsync(v => v.Id, v => v, cancellationToken: ct);

		//Assert
		result0.Should().Contain(testObj);
	}
}