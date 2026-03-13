using CommonFramework.DependencyInjection;

using GenericQueryable.Fetching;
using GenericQueryable.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GenericQueryable.DependencyInjection;

public class GenericQueryableBuilder : IGenericQueryableBuilder, IServiceInitializer
{
    private Type fetchServiceType = typeof(IgnoreFetchService);

    private Type targetMethodExtractorType = typeof(QueryableTargetMethodExtractor);

    private readonly List<Type> fetchRuleExpanderTypeList = [];

    private readonly List<FetchRuleHeaderInfo> fetchRuleHeaderInfoList = [];

    public void Initialize(IServiceCollection services)
    {
        if (!services.AlreadyInitialized<IGenericQueryableExecutor>())
        {
            services.TryAddSingleton<IGenericQueryableExecutor, GenericQueryableExecutor>();
            services.TryAddSingleton<IMethodRedirector, MethodRedirector>();

            services.AddSingleton<IFetchRuleExpander, FetchRuleHeaderExpander>();
            services.AddSingleton<IFetchRuleExpander, UntypedFetchExpander>();

            services.AddKeyedSingleton<IFetchRuleExpander, RootFetchRuleExpander>(RootFetchRuleExpander.Key);
        }

        if (services.AlreadyInitialized<IFetchService, IgnoreFetchService>())
        {
            services.ReplaceSingleton(typeof(IFetchService), this.fetchServiceType);
        }
        else
        {
            services.AddSingleton(typeof(IFetchService), this.fetchServiceType);
        }

        if (services.AlreadyInitialized<ITargetMethodExtractor, QueryableTargetMethodExtractor>())
        {
            services.ReplaceSingleton(typeof(ITargetMethodExtractor), this.targetMethodExtractorType);
        }
        else
        {
            services.AddSingleton(typeof(ITargetMethodExtractor), this.targetMethodExtractorType);
        }

        foreach (var fetchRuleExpanderType in this.fetchRuleExpanderTypeList)
        {
            services.AddSingleton(typeof(IFetchRuleExpander), fetchRuleExpanderType);
        }

        foreach (var fetchRuleHeaderInfo in this.fetchRuleHeaderInfoList)
        {
            services.AddSingleton(fetchRuleHeaderInfo);
        }
    }

    public IGenericQueryableBuilder SetFetchService<TFetchService>()
        where TFetchService : IFetchService
    {
        this.fetchServiceType = typeof(TFetchService);

        return this;
    }

    public IGenericQueryableBuilder AddFetchRule<TSource>(FetchRuleHeader<TSource> header, PropertyFetchRule<TSource> implementation)
    {
        this.fetchRuleHeaderInfoList.Add(new FetchRuleHeaderInfo<TSource>(header, implementation));

        return this;
    }

    public IGenericQueryableBuilder SetTargetMethodExtractor<TTargetMethodExtractor>()
        where TTargetMethodExtractor : ITargetMethodExtractor
    {
        this.targetMethodExtractorType = typeof(TTargetMethodExtractor);

        return this;
    }

    public IGenericQueryableBuilder AddFetchRuleExpander<TFetchRuleExpander>()
        where TFetchRuleExpander : IFetchRuleExpander
    {
        this.fetchRuleExpanderTypeList.Add(typeof(TFetchRuleExpander));

        return this;
    }
}