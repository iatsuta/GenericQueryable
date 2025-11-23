using GenericQueryable.Services;

using Microsoft.EntityFrameworkCore;

namespace GenericQueryable.EntityFramework;

public class EfTargetMethodExtractor() : TargetMethodExtractor([typeof(EntityFrameworkQueryableExtensions)]);