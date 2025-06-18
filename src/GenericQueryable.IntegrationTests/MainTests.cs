using GenericQueryable.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.IntegrationTests;

public class MainTests
{
    [Fact]
    public async Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked()
    {
        // Arrange

        var sp = new ServiceCollection()
            .AddDbContext<TestDbContext>(optionsBuilder => optionsBuilder
                    .UseSqlite("Data Source=test.db")
                    .UseGenericQueryable(),
                contextLifetime: ServiceLifetime.Singleton,
                optionsLifetime: ServiceLifetime.Singleton)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        var dbContext = sp.GetRequiredService<TestDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var testSet = dbContext.Set<TestObject>();

        var testObj = new TestObject { Id = Guid.NewGuid() };

        await testSet.AddAsync(testObj);

        await dbContext.SaveChangesAsync();

        // Act
        var result = await testSet.GenericToListAsync();

        //Assert
        result.Should().Contain(testObj);
    }
}