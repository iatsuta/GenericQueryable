using System.Linq.Expressions;

namespace GenericQueryable.IntegrationTests.Environment;

public interface INHibExpressionVisitorSource
{
    ExpressionVisitor Visitor { get; }
}