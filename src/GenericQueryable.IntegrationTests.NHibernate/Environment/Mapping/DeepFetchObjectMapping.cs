using FluentNHibernate.Mapping;
using GenericQueryable.IntegrationTests.Domain;

namespace GenericQueryable.IntegrationTests.Environment.Mapping;

public class DeepFetchObjectMapping : ClassMap<DeepFetchObject>
{
    public DeepFetchObjectMapping()
    {
        this.Schema("app");

        this.DynamicUpdate();

        this.Id(x => x.Id).GeneratedBy.GuidComb();

        this.References(x => x.FetchObject).Column($"{nameof(DeepFetchObject.FetchObject)}Id");
    }
}