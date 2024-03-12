namespace Intelsoft.Common.Result.UnitTests;

public class ErrorUnitTests
{
    [Theory]
    [InlineData("General.Failure", "A failure has occurred.", ErrorType.Failure)]
    public void Create_Failure_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.Failure();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Theory]
    [InlineData("General.Validation", "A validation error has occurred.", ErrorType.Validation)]
    public void Create_Validation_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.Validation();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Theory]
    [InlineData("General.Unauthorized", "An 'Unauthorized' error has occurred.", ErrorType.Unauthorized)]
    public void Create_Unauthorized_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.Unauthorized();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Theory]
    [InlineData("General.Forbidden", "A 'Forbidden' error has occurred.", ErrorType.Forbidden)]
    public void Create_Forbidden_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.Forbidden();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Theory]
    [InlineData("General.NotFound", "A 'Not Found' error has occurred.", ErrorType.NotFound)]
    public void Create_NotFound_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.NotFound();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Theory]
    [InlineData("General.Conflict", "A conflict error has occurred.", ErrorType.Conflict)]
    public void Create_Conflict_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.Conflict();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Theory]
    [InlineData("General.Unexpected", "An unexpected error has occurred.", ErrorType.Unexpected)]
    public void Create_Unexpected_ReturnsExpectedError(string expectedCode, string expectedDescription,
        ErrorType expectedErrorType)
    {
        var error = Error.Unexpected();

        Assert.Equal(expectedCode, error.Code);
        Assert.Equal(expectedDescription, error.Description);
        Assert.Equal(expectedErrorType, error.ErrorType);
    }

    [Fact]
    public void Create_CustomError_ReturnsExpectedValues()
    {
        var code = "Custom.ErrorCode";
        var description = "Custom error description";
        var error = Error.Create(code, description, ErrorType.Failure);

        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Failure, error.ErrorType);
    }
}