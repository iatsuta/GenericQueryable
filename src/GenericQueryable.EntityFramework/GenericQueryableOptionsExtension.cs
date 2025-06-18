﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GenericQueryable.EntityFramework;

public class GenericQueryableOptionsExtension : IDbContextOptionsExtension
{
    public GenericQueryableOptionsExtension()
    {
        this.Info = new ExtensionInfo(this);
    }

    public DbContextOptionsExtensionInfo Info { get; }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton<IGenericQueryableExecutor, EfGenericQueryableExecutor>();

        services.Replace(ServiceDescriptor.Scoped<IAsyncQueryProvider, VisitedEfQueryProvider>());
    }

    public void Validate(IDbContextOptions options)
    {
    }

    private sealed class ExtensionInfo(IDbContextOptionsExtension extension) : DbContextOptionsExtensionInfo(extension)
    {
        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "using GenericQueryable ";

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return true;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["GenericQueryable"] = "1";
        }
    }
}