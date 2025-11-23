using GenericQueryable.Services;

using Microsoft.EntityFrameworkCore;

namespace GenericQueryable.EntityFramework;

public class EfTargetMethodExtractor : TargetMethodExtractor
{
	protected override IReadOnlyList<Type> ExtensionsTypes { get; } = [typeof(EntityFrameworkQueryableExtensions)];
}