using Microsoft.EntityFrameworkCore;

namespace GenericQueryable.IntegrationTests;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestObject>();

        base.OnModelCreating(modelBuilder);
    }
}