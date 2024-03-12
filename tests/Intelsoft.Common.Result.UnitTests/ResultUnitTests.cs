namespace Intelsoft.Common.Result.UnitTests;

public class ResultUnitTests
{
    [Fact]
    public void Success_Result_ShouldBeSuccessful()
    {
        var result = Result.Success;

        Assert.True(result.IsSuccessful);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Failure_Result_ShouldContainErrors()
    {
        var error = Error.Failure("Test", "Test failure");
        var result = (Result)error;

        Assert.False(result.IsSuccessful);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors[0]);
    }

    [Fact]
    public void Implicit_Conversion_From_Error_ShouldCreateFailedResult()
    {
        var error = Error.Failure();
        Result result = error;

        Assert.False(result.IsSuccessful);
        Assert.Contains(error, result.Errors);
    }

    [Fact]
    public void Implicit_Conversion_From_ErrorArray_ShouldCreateFailedResultWithMultipleErrors()
    {
        var errors = new[] { Error.Failure(), Error.NotFound() };
        Result result = errors;

        Assert.False(result.IsSuccessful);
        Assert.Equal(2, result.Errors.Length);
    }

    [Fact]
    public void GetFirstErrorOrSuccess_AllSuccess_ReturnsSuccess()
    {
        var success1 = Result.Success;
        var success2 = Result.Success;

        var result = Result.GetFirstErrorOrSuccess(success1, success2);

        Assert.True(result.IsSuccessful);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetFirstErrorOrSuccess_WithOneError_ReturnsFirstError()
    {
        var success = Result.Success;
        var error = Error.Failure("Error1", "First error");
        var resultWithError = (Result)error;

        var result = Result.GetFirstErrorOrSuccess(success, resultWithError);

        Assert.False(result.IsSuccessful);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors[0]);
    }

    [Fact]
    public void GetFirstErrorOrSuccess_WithMultipleErrors_ReturnsFirstError()
    {
        var error1 = Error.Failure("Error1", "First error");
        var resultWithError1 = (Result)error1;
        var error2 = Error.Validation("Error2", "Second error");
        var resultWithError2 = (Result)error2;

        var result = Result.GetFirstErrorOrSuccess(resultWithError1, resultWithError2);

        Assert.False(result.IsSuccessful);
        Assert.Single(result.Errors);
        Assert.Equal(error1, result.Errors[0]);
    }

    [Fact]
    public void GetFirstErrorOrSuccess_WithSuccessAndError_ReturnsError()
    {
        var error = Error.Unexpected("Error", "Error occurred");
        var resultWithError = (Result)error;
        var success = Result.Success;

        var result = Result.GetFirstErrorOrSuccess(success, resultWithError);

        Assert.False(result.IsSuccessful);
        Assert.Single(result.Errors);
        Assert.Equal(error, result.Errors[0]);
    }

    [Fact]
    public void GetFirstErrorOrSuccess_WithMultipleSuccessesAndOneErrorAtDifferentPositions_ReturnsError()
    {
        var success1 = Result.Success;
        var success2 = Result.Success;
        var error = Error.Unexpected("Error", "Error occurred");
        var resultWithError = (Result)error;

        var resultMiddleError = Result.GetFirstErrorOrSuccess(success1, resultWithError, success2);
        var resultLastError = Result.GetFirstErrorOrSuccess(success1, success2, resultWithError);

        Assert.False(resultMiddleError.IsSuccessful);
        Assert.Single(resultMiddleError.Errors);
        Assert.Equal(error, resultMiddleError.Errors[0]);

        Assert.False(resultLastError.IsSuccessful);
        Assert.Single(resultLastError.Errors);
        Assert.Equal(error, resultLastError.Errors[0]);
    }
}