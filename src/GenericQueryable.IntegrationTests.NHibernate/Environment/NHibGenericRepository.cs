using CommonFramework;
using CommonFramework.GenericRepository;
using CommonFramework.IdentitySource;

using NHibernate;

namespace GenericQueryable.IntegrationTests.Environment;

public class NHibGenericRepository(
    ISession session,
    IIdentityInfoSource identityInfoSource) : IGenericRepository
{
    public Task SaveAsync<TDomainObject>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        var identityInfo = identityInfoSource.GetIdentityInfo<TDomainObject>();

        return new Func<TDomainObject, CancellationToken, Task>(this.SaveAsync<TDomainObject, Ignore>)
            .CreateGenericMethod(identityInfo.DomainObjectType, identityInfo.IdentityType)
            .Invoke<Task>(this, domainObject, cancellationToken);
    }

    public async Task SaveAsync<TDomainObject, TIdent>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class
        where TIdent : notnull
    {
        if (!session.Contains(domainObject))
        {
            var identityInfo = identityInfoSource.GetIdentityInfo<TDomainObject, TIdent>();

            var id = identityInfo.Id.Getter(domainObject);

            if (!EqualityComparer<TIdent>.Default.Equals(id, default))
            {
                await session.SaveAsync(domainObject, id, cancellationToken);
                await session.FlushAsync(cancellationToken);

                return;
            }
        }

        await session.SaveOrUpdateAsync(domainObject, cancellationToken);
        await session.FlushAsync(cancellationToken);
    }

    public Task RemoveAsync<TDomainObject>(TDomainObject domainObject, CancellationToken cancellationToken)
        where TDomainObject : class => session.DeleteAsync(domainObject, cancellationToken);
}