using CommonFramework.DependencyInjection;

using GenericQueryable.DependencyInjection;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.EntityFramework;

public class GenericQueryableOptionsExtension : IDbContextOptionsExtension
{
    private readonly Action<IGenericQueryableSetup>? setupAction;

    public GenericQueryableOptionsExtension(Action<IGenericQueryableSetup>? setupAction)
    {
        this.setupAction = setupAction;
        this.Info = new ExtensionInfo(this);
    }

	public DbContextOptionsExtensionInfo Info { get; }

	public void ApplyServices(IServiceCollection services)
	{
		services.AddGenericQueryable(v =>
        {
            v.SetFetchService<EfFetchService>().SetTargetMethodExtractor<EfTargetMethodExtractor>();

			setupAction?.Invoke(v);
        });

        services.ReplaceScoped<IAsyncQueryProvider, VisitedEfQueryProvider>();
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