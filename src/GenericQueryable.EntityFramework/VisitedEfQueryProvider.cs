using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GenericQueryable.EntityFramework;

public class VisitedEfQueryProvider(IQueryCompiler queryCompiler, IGenericQueryableExecutor executor)
    : EntityQueryProvider(queryCompiler), IGenericQueryProvider
{
    public IGenericQueryableExecutor Executor { get; } = executor;
}