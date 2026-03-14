using CommonFramework;
using CommonFramework.GenericRepository;

using NHibernate;

namespace GenericQueryable.IntegrationTests.Environment;

public class NHibGenericRepository(
    IServiceProxyFactory serviceProxyFactory,
    ISession session) : IGenericRepository
{
    public async Task SaveAsync<TDomainObject>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        await serviceProxyFactory.Create<IDomainObjectSaveStrategy<TDomainObject>>().SaveAsync(session, domainObject, cancellationToken);

        await session.FlushAsync(cancellationToken);
    }

    public Task RemoveAsync<TDomainObject>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class => session.DeleteAsync(domainObject, cancellationToken);
}