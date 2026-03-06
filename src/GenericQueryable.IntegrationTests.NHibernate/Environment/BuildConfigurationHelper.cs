using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

using GenericQueryable.IntegrationTests.Environment.Mapping;
using GenericQueryable.NHibernate;

using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq.Functions;
using NHibernate.Tool.hbm2ddl;

namespace GenericQueryable.IntegrationTests.Environment;

public static class BuildConfigurationHelper
{
    public static Configuration BuildConfiguration(string connectionString)
    {
        var cfg = new Configuration();

        Fluently
            .Configure(cfg)
            .Database(SQLiteConfiguration.Standard
                .Dialect<SQLiteDialect>()
                .Driver<SQLite20Driver>()
                .ConnectionString(connectionString))
            .Mappings(
                m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<DeepFetchObjectMapping>()
                        .Conventions.AddFromAssemblyOf<EnumConvention>();
                })
            .ExposeConfiguration(
                c =>
                {
                    c.Properties.Add(global::NHibernate.Cfg.Environment.LinqToHqlGeneratorsRegistry, typeof(DefaultLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);

                    //c.Properties.Add(global::NHibernate.Cfg.Environment.SqlExceptionConverter, typeof(SQLStateConverter).AssemblyQualifiedName);

                    c.Properties.Add(global::NHibernate.Cfg.Environment.CommandTimeout, "1200");

                    c.Properties.Add(global::NHibernate.Cfg.Environment.SqlTypesKeepDateTime, "true");

                    c.Properties.Add(global::NHibernate.Cfg.Environment.ShowSql, "true");
                })
            .BuildConfiguration();

        cfg.SessionFactory().ParsingLinqThrough<VisitedNHibQueryProvider>();


        SchemaMetadataUpdater.QuoteTableAndColumns(cfg, Dialect.GetDialect(cfg.Properties));

        var schemaExport = new SchemaExport(cfg);
        schemaExport.Create(false, true);

        return cfg;
    }

    private class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) =>
            criteria.Expect(
                x => x.Property.PropertyType.IsEnum
                     || (x.Property.PropertyType.IsGenericType
                         && x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                         && x.Property.PropertyType.GetGenericArguments()[0].IsEnum));

        public void Apply(IPropertyInstance instance) => instance.CustomType(instance.Property.PropertyType);
    }
}