using GenericQueryable.IntegrationTests.Environment;

namespace GenericQueryable.IntegrationTests;

public class MainTests() : MainTestsBase(EfTestEnvironment.Instance)
{
    [Fact]
    public override Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked() =>
        base.DefaultGenericQueryable_InvokeToListAsync_MethodInvoked();
}