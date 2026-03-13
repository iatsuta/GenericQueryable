using GenericQueryable.Services;

using NHibernate.Linq;

namespace GenericQueryable.NHibernate;

public class NHibTargetMethodExtractor() : TargetMethodExtractor([typeof(LinqExtensionMethods), typeof(NHibLinqExtensions)]);