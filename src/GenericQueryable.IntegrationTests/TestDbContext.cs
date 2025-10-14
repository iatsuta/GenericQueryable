using GenericQueryable.IntegrationTests.Domain;

using Microsoft.EntityFrameworkCore;

namespace GenericQueryable.IntegrationTests;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestObject>();

        modelBuilder.Entity<FetchObject>();

        modelBuilder.Entity<DeepFetchObject>();

        base.OnModelCreating(modelBuilder);
    }
}