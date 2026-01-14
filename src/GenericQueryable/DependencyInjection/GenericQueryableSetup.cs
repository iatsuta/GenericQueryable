using GenericQueryable.Fetching;
using GenericQueryable.Services;

using Microsoft.Extensions.DependencyInjection;

namespace GenericQueryable.DependencyInjection;

public class GenericQueryableSetup : IGenericQueryableSetup
{
    private Type targetMethodExtractorType = typeof(SyncTargetMethodExtractor);

    private Type fetchServiceType = typeof(IgnoreFetchService);

    private readonly List<Type> fetchRuleExpanderTypeList = [typeof(FetchRuleHeaderExpander)];

    private readonly List<FetchRuleHeaderInfo> fetchRuleHeaderInfoList = new();

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IGenericQueryableExecutor, GenericQueryableExecutor>();
        services.AddSingleton<IMethodRedirector, MethodRedirector>();

        services.AddSingleton(typeof(IFetchService), this.fetchServiceType);
        services.AddSingleton(typeof(ITargetMethodExtractor), this.targetMethodExtractorType);

        services.AddKeyedSingleton<IFetchRuleExpander, RootFetchRuleExpander>(RootFetchRuleExpander.Key);

        foreach (var fetchRuleExpanderType in this.fetchRuleExpanderTypeList)
        {
            services.AddSingleton(typeof(IFetchRuleExpander), fetchRuleExpanderType);
        }

        foreach (var fetchRuleHeaderInfo in this.fetchRuleHeaderInfoList)
        {
            services.AddSingleton(fetchRuleHeaderInfo);
        }
    }

    public IGenericQueryableSetup SetFetchService<TFetchService>()
        where TFetchService : IFetchService
    {
        this.fetchServiceType = typeof(TFetchService);

        return this;
    }

    public IGenericQueryableSetup AddFetchRule<TSource>(FetchRuleHeader<TSource> header, PropertyFetchRule<TSource> implementation)
    {
        this.fetchRuleHeaderInfoList.Add(new FetchRuleHeaderInfo<TSource>(header, implementation));

        return this;
    }

    public IGenericQueryableSetup SetTargetMethodExtractor<TTargetMethodExtractor>()
        where TTargetMethodExtractor : ITargetMethodExtractor
    {
        this.targetMethodExtractorType = typeof(TTargetMethodExtractor);

        return this;
    }

    public IGenericQueryableSetup AddFetchRuleExpander<TFetchRuleExpander>()
        where TFetchRuleExpander : IFetchRuleExpander
    {
        this.fetchRuleExpanderTypeList.Add(typeof(TFetchRuleExpander));

        return this;
    }
}