namespace GenericQueryable;

public interface IGenericQueryProvider : IQueryProvider
{
    IGenericQueryableExecutor Executor { get; }
}