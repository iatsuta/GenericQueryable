using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GenericQueryable.EntityFramework;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseGenericQueryable(this DbContextOptionsBuilder optionsBuilder)
    {
        var extension = optionsBuilder.Options.FindExtension<GenericQueryableOptionsExtension>()
                        ?? new GenericQueryableOptionsExtension();

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}