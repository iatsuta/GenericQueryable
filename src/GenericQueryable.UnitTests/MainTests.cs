namespace GenericQueryable.UnitTests;

public class MainTests
{
    private readonly CancellationToken ct = TestContext.Current.CancellationToken;

	[Fact]
    public async Task DefaultGenericQueryable_InvokeSumAsync_MethodInvoked()
    {
		// Arrange
		var baseSource = new decimal?[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericSumAsync(cancellationToken: ct);

        //Assert
        result.Should().Be(baseSource.Sum());
	}

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToArrayAsync_MethodInvoked()
    {
		// Arrange
		var baseSource = new[] { 1, 2, 3 };
	    var qSource = baseSource.AsQueryable();

	    // Act
	    var result = await qSource.GenericToArrayAsync(cancellationToken: ct);

	    //Assert
	    result.Should().BeEquivalentTo(baseSource);
    }

	[Fact]
    public async Task DefaultGenericQueryable_InvokeToListAsync_MethodInvoked()
    {
		// Arrange
		var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToListAsync(cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource);
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToHashSetAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToHashSetAsync(cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource);
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToHashSetWithComparerAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToHashSetAsync(EqualityComparer<int>.Default, cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource);
    }


    [Fact]
    public async Task DefaultGenericQueryable_InvokeToDictionaryAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToDictionaryAsync(v => v, cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource.ToDictionary(v => v));
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToDictionaryWithComparerAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToDictionaryAsync(v => v, EqualityComparer<int>.Default, cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource.ToDictionary(v => v));
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToDictionaryWithElementSelectorAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToDictionaryAsync(v => v, v => v, cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource.ToDictionary(v => v));
    }

    [Fact]
    public async Task DefaultGenericQueryable_InvokeToDictionaryWithElementSelectorAndComparerAsync_MethodInvoked()
    {
        // Arrange
        var baseSource = new[] { 1, 2, 3 };
        var qSource = baseSource.AsQueryable();

        // Act
        var result = await qSource.GenericToDictionaryAsync(v => v, v => v, EqualityComparer<int>.Default, cancellationToken: ct);

        //Assert
        result.Should().BeEquivalentTo(baseSource.ToDictionary(v => v));
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
        var result = await qSource.GenericSingleOrDefaultAsync(_ => true, cancellationToken: ct);

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
            .GenericSingleOrDefaultAsync(_ => true, cancellationToken: ct);

        //Assert
        result.Should().Be(baseSource);
    }
}