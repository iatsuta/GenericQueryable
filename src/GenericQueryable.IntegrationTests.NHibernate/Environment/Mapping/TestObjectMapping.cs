using FluentNHibernate.Mapping;
using GenericQueryable.IntegrationTests.Domain;

namespace GenericQueryable.IntegrationTests.Environment.Mapping;

public class TestObjectMapping : ClassMap<TestObject>
{
    public TestObjectMapping()
    {
        this.Schema("app");

        this.DynamicUpdate();

        this.Id(x => x.Id).GeneratedBy.GuidComb();

        this.References(x => x.FetchObject).Column($"{nameof(TestObject.FetchObject)}Id");

        this.HasMany(x => x.DeepFetchObjects).AsSet().Inverse().Cascade.AllDeleteOrphan();
    }
}