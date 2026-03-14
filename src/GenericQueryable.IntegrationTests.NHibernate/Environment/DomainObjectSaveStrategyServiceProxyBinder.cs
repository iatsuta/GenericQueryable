using CommonFramework;
using CommonFramework.IdentitySource;

namespace GenericQueryable.IntegrationTests.Environment;

public class DomainObjectSaveStrategyServiceProxyBinder<TDomainObject>(IIdentityInfoSource identityInfoSource) : IServiceProxyBinder
{
    public Type GetTargetServiceType()
    {
        var identityInfo = identityInfoSource.GetIdentityInfo(typeof(TDomainObject));

        return typeof(DomainObjectSaveStrategy<,>).MakeGenericType(identityInfo.DomainObjectType, identityInfo.IdentityType);
    }
}