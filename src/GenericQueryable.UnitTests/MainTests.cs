using GenericQueryable.Fetching;

namespace GenericQueryable.UnitTests;

public class MainTests
{
    [Fact]
    public async Task DefaultGenericQueryable_InvokeSumAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new decimal?[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericSumAsync();

        //Assert
        result.Should().Be(baseSource.Sum());
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToListAsync();

        //Assert
        result.Should().BeEquivalentTo(baseSource);
    }

    [Fact]
    public void DefaultGenericQueryable_InvokeToList_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = qSource.ToList();

        //Assert
        result.Should().BeEquivalentTo(baseSource);
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeSingleOrDefaultAsync_CollisionResolved()
    {
        // Arrange
        var baseSource = 1;
        var qSource = new[] { baseSource }.AsQueryable();

        // Act
        var result = await qSource.GenericSingleOrDefaultAsync(_ => true);

        //Assert
        result.Should().Be(baseSource);
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeFetch_FetchIgnored()
    {
        // Arrange
        var baseSource = "abc";
        var qSource = new[] { baseSource }.AsQueryable();

        // Act
        var result = await qSource.WithFetch(nameof(string.Length))
            .GenericSingleOrDefaultAsync(_ => true);

        //Assert
        result.Should().Be(baseSource);
    }
}