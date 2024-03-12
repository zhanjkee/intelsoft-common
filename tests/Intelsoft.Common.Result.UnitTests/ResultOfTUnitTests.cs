namespace Intelsoft.Common.Result.UnitTests;

public class ResultOfTUnitTests
{
    [Fact]
    public void Success_ResultOfT_ShouldContainValue()
    {
        var result = Result.Create(42);

        Assert.True(result.IsSuccessful);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_ResultOfT_ShouldContainErrors()
    {
        var error = Error.Failure("Test", "Test failure");
        var result = (Result<int>)error;

        Assert.False(result.IsSuccessful);
        Assert.NotEmpty(result.Value.ToString());
        Assert.Contains(error, result.Errors);
    }

    [Fact]
    public void Implicit_Conversion_From_ValueToResultOfT_ShouldCreateSuccessfulResult()
    {
        Result<int> result = 100;

        Assert.True(result.IsSuccessful);
        Assert.Equal(100, result.Value);
    }

    [Fact]
    public void Implicit_Conversion_From_ErrorToResultOfT_ShouldCreateFailedResult()
    {
        var error = Error.Validation();
        Result<int> result = error;

        Assert.False(result.IsSuccessful);
        Assert.Equal(default, result.Value);
        Assert.Contains(error, result.Errors);
    }

    [Fact]
    public void Implicit_Conversion_From_ErrorArrayToResultOfT_ShouldCreateFailedResultWithMultipleErrors()
    {
        var errors = new[] { Error.Unauthorized(), Error.Forbidden() };
        Result<string> result = errors;

        Assert.False(result.IsSuccessful);
        Assert.Null(result.Value);
        Assert.Equal(errors.Length, result.Errors.Length);
    }
}