using CommonFramework.DependencyInjection;
using CommonFramework.GenericRepository;
using CommonFramework.IdentitySource.DependencyInjection;

using GenericQueryable.NHibernate;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace GenericQueryable.IntegrationTests.Environment;

public class TestEnvironmentImpl : TestEnvironment
{
    protected override IServiceCollection InitializeServices(IServiceCollection services)
    {
        return services

            .AddIdentitySource()
            .AddSingleton(BuildConfigurationHelper.BuildConfiguration("Data Source=TestSystem.sqlite"))
            .AddSingletonFrom((global::NHibernate.Cfg.Configuration cfg) => cfg.BuildSessionFactory())
            .AddScopedFrom((ISessionFactory sessionFactory) => sessionFactory.OpenSession())

            .AddSingleton(typeof(IDomainObjectSaveStrategy<>), typeof(DomainObjectSaveStrategy<>))
            .BindServiceProxy(typeof(IDomainObjectSaveStrategy<>), typeof(DomainObjectSaveStrategyServiceProxyBinder<>))

            .AddScoped<IGenericRepository, NHibGenericRepository>()
            .AddScoped<IQueryableSource, NHibQueryableSource>()
            .AddNHibernateGenericQueryable(this.SetupGenericQueryable);
    }

    public override async Task InitializeDatabase()
    {
    }

    public static TestEnvironmentImpl Instance { get; } = new ();
}