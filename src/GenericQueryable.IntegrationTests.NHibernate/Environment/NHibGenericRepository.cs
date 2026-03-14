using CommonFramework.GenericRepository;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace GenericQueryable.IntegrationTests.Environment;

public class NHibGenericRepository(
    IServiceProvider serviceProvider,
    ISession session) : IGenericRepository
{
    public async Task SaveAsync<TDomainObject>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        await serviceProvider.GetRequiredService<IDomainObjectSaveStrategy<TDomainObject>>().SaveAsync(session, domainObject, cancellationToken);

        await session.FlushAsync(cancellationToken);
    }

    public Task RemoveAsync<TDomainObject>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class => session.DeleteAsync(domainObject, cancellationToken);
}