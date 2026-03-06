using CommonFramework.GenericRepository;

using GenericQueryable.EntityFramework;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.IntegrationTests.Environment;

public class TestEnvironmentImpl : TestEnvironment
{
    protected override IServiceCollection InitializeServices(IServiceCollection services)
    {
        return services
            .AddDbContext<TestDbContext>(optionsBuilder => optionsBuilder
                .UseSqlite("Data Source=test.db")
                .UseGenericQueryable(this.SetupGenericQueryable))
            .AddScoped<IGenericRepository, EfGenericRepository>()
            .AddScoped<IQueryableSource, EfQueryableSource>();
    }

    public override async Task InitializeDatabase()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        await using var scope = this.RootServiceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();

        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    public static TestEnvironmentImpl Instance { get; } = new ();
}