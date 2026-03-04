using GenericQueryable.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GenericQueryable.EntityFramework;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseGenericQueryable(this DbContextOptionsBuilder optionsBuilder, Action<IGenericQueryableBuilder>? setupAction = null)
    {
        var extension = optionsBuilder.Options.FindExtension<GenericQueryableOptionsExtension>()
                        ?? new GenericQueryableOptionsExtension(setupAction);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}