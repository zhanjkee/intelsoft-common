namespace Intelsoft.Common.Result.UnitTests;

public class ResultExtensionsUnitTests
{
    [Fact]
    public void Validate_WithSuccessfulResultAndTrueCondition_ShouldReturnOriginalResult()
    {
        var result = Result.Create(5);
        var validatedResult = result.Validate(value => value > 0);

        Assert.True(validatedResult.IsSuccessful);
    }

    [Fact]
    public void Validate_WithSuccessfulResultAndFalseCondition_ShouldReturnFailure()
    {
        var result = Result.Create(5);
        var validatedResult = result.Validate(value => value < 0, Error.Validation());

        Assert.False(validatedResult.IsSuccessful);
        Assert.Contains(validatedResult.Errors, e => e.ErrorType == ErrorType.Validation);
    }

    [Fact]
    public async Task ValidateAsync_WithSuccessfulResultAndTrueCondition_ShouldReturnOriginalResult()
    {
        var resultTask = Task.FromResult(Result.Create(5));
        var validatedResult = await resultTask.Validate(value => value > 0);

        Assert.True(validatedResult.IsSuccessful);
    }

    [Fact]
    public async Task ValidateAsync_WithSuccessfulResultAndFalseCondition_ShouldReturnFailure()
    {
        var resultTask = Task.FromResult(Result.Create(5));
        var validatedResult = await resultTask.Validate(value => value < 0, Error.Validation());

        Assert.False(validatedResult.IsSuccessful);
        Assert.Contains(validatedResult.Errors, e => e.ErrorType == ErrorType.Validation);
    }

    [Fact]
    public void Validate_WithSuccessfulResultAndFalseCondition_ShouldReturnFailureWithError()
    {
        var result = Result.Create(5);
        var error = Error.Validation("Invalid", "Value is not valid");
        var validatedResult = result.Validate(value => value > 10, error);

        Assert.False(validatedResult.IsSuccessful);
        Assert.Contains(validatedResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                     e.Code == error.Code &&
                                                     e.Description == error.Description);
    }

    [Fact]
    public void Validate_WithFailedResult_ShouldReturnOriginalErrorsWithoutValidation()
    {
        var error = Error.Failure("InitialFailure", "Initial failure occurred");
        Result<int> result = error;
        var validationError = Error.Validation("ValidationFailed", "Validation failed");

        var validatedResult = result.Validate(value => value > 0, validationError);

        Assert.False(validatedResult.IsSuccessful);
        Assert.Contains(validatedResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                     e.Code == error.Code &&
                                                     e.Description == error.Description);

        Assert.DoesNotContain(validatedResult.Errors, e => e.ErrorType == validationError.ErrorType &&
                                                           e.Code == validationError.Code &&
                                                           e.Description == validationError.Description);
    }

    [Fact]
    public void Validate_WithSuccessfulValidationAndMultipleErrors_ShouldNotApplyErrors()
    {
        var result = Result.Create(20);
        var error1 = Error.Validation("Error1", "This error should not be applied");
        var error2 = Error.Validation("Error2", "This error should also not be applied");
        var validatedResult = result.Validate(value => value > 10, error1, error2);

        Assert.True(validatedResult.IsSuccessful);
        Assert.Empty(validatedResult.Errors);
    }

    [Fact]
    public async Task ValidateAsync_WithSuccessfulResultAndFalseCondition_ShouldReturnFailureWithError()
    {
        var resultTask = Task.FromResult(Result.Create(5));
        var error = Error.Validation("Invalid", "Value is not valid");
        var validatedResult = await resultTask.Validate(value => value > 10, error);

        Assert.False(validatedResult.IsSuccessful);
        Assert.Contains(validatedResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                     e.Code == error.Code &&
                                                     e.Description == error.Description);
    }

    [Fact]
    public async Task ValidateAsync_WithFailedResult_ShouldReturnOriginalErrorsWithoutValidation()
    {
        var initialError = Error.Failure("InitialFailure", "Initial failure occurred");
        Result<int> result = initialError;
        var resultTask = Task.FromResult(result);
        var validationError = Error.Validation("ValidationFailed", "Validation failed");

        var validatedResult = await resultTask.Validate(value => value > 0, validationError);

        Assert.False(validatedResult.IsSuccessful);
        Assert.Contains(validatedResult.Errors, e => e.ErrorType == initialError.ErrorType &&
                                                     e.Code == initialError.Code &&
                                                     e.Description == initialError.Description);

        Assert.DoesNotContain(validatedResult.Errors, e => e.ErrorType == validationError.ErrorType &&
                                                           e.Code == validationError.Code &&
                                                           e.Description == validationError.Description);
    }

    [Fact]
    public void Map_WithSuccessfulResult_ShouldMapValue()
    {
        var result = Result.Create(5);
        var mappedResult = result.Map(value => value.ToString());

        Assert.True(mappedResult.IsSuccessful);
        Assert.Equal("5", mappedResult.Value);
    }

    [Fact]
    public void Map_WithFailure_ShouldReturnFailureWithSameErrors()
    {
        var error = Error.Validation();
        Result<int> result = error;
        var mappedResult = result.Map(value => value.ToString());

        Assert.False(mappedResult.IsSuccessful);
        Assert.Contains(mappedResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                  e.Code == error.Code &&
                                                  e.Description == error.Description);
    }

    [Fact]
    public async Task MapAsync_WithSuccessfulResult_ShouldMapValue()
    {
        var resultTask = Task.FromResult(Result.Create(5));
        var mappedResult = await resultTask.Map(value => value.ToString());

        Assert.True(mappedResult.IsSuccessful);
        Assert.Equal("5", mappedResult.Value);
    }

    [Fact]
    public async Task MapAsync_WithFailure_ShouldReturnFailureWithSameErrors()
    {
        var error = Error.Validation();
        Result<int> result = error;
        var resultTask = Task.FromResult(result);
        var mappedResult = await resultTask.Map(value => value.ToString());

        Assert.False(mappedResult.IsSuccessful);
        Assert.Contains(mappedResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                  e.Code == error.Code &&
                                                  e.Description == error.Description);
    }

    [Fact]
    public void Bind_WithSuccessfulResult_ShouldBindAndReturnNewResult()
    {
        var result = Result.Create(5);
        var bindResult = result.Bind(value => Result.Create(value > 10));

        Assert.True(bindResult.IsSuccessful);
    }

    [Fact]
    public void Bind_WithFailure_ShouldReturnInitialFailure()
    {
        var error = Error.Validation();
        Result<int> result = error;
        var bindResult = result.Bind(_ => Result.Create(true));

        Assert.False(bindResult.IsSuccessful);
        Assert.Contains(bindResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                e.Code == error.Code &&
                                                e.Description == error.Description);
    }

    [Fact]
    public async Task BindAsync_WithSuccessfulResult_ShouldBindAndReturnNewResult()
    {
        var resultTask = Task.FromResult(Result.Create(5));
        var bindResult = await resultTask.Bind(value => Task.FromResult(Result.Create(value > 10)));

        Assert.True(bindResult.IsSuccessful);
    }

    [Fact]
    public async Task BindAsync_WithFailure_ShouldReturnInitialFailure()
    {
        var error = Error.Validation();
        Result<int> result = error;
        var resultTask = Task.FromResult(result);
        var bindResult = await resultTask.Bind(_ => Task.FromResult(Result.Create(true)));

        Assert.False(bindResult.IsSuccessful);
        Assert.Contains(bindResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                e.Code == error.Code &&
                                                e.Description == error.Description);
    }

    [Fact]
    public void Bind_ResultWithSuccess_ShouldApplyFunction()
    {
        var initialResult = Result.Create(5);
        var boundResult = initialResult.Bind(value => Result.Create(value > 3));

        Assert.True(boundResult.IsSuccessful);
    }

    [Fact]
    public void Bind_ResultWithFailure_ShouldNotApplyFunctionAndReturnFailure()
    {
        var error = Error.Failure("Error", "Initial error");
        Result<int> result = error;
        var boundResult = result.Bind(_ => Result.Success);

        Assert.False(boundResult.IsSuccessful);
        Assert.Contains(boundResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                 e.Code == error.Code &&
                                                 e.Description == error.Description);
    }

    [Fact]
    public async Task BindAsync_ResultWithFailure_ShouldNotApplyFunctionAndReturnFailure()
    {
        var error = Error.Failure("Error", "Initial error");
        Result<int> result = error;
        var boundResult = await result.Bind(_ => Task.FromResult(Result.Success));

        Assert.False(boundResult.IsSuccessful);
        Assert.Contains(boundResult.Errors, e => e.ErrorType == error.ErrorType &&
                                                 e.Code == error.Code &&
                                                 e.Description == error.Description);
    }

    [Fact]
    public void Bind_ResultWithSuccess_ShouldApplyFunctionAndReturnFailure()
    {
        var error = Error.Failure("Error", "Initial error");
        var result = Result.Create(5);
        var boundResult = result.Bind(_ => error);

        Assert.False(boundResult.IsSuccessful);
    }

    [Fact]
    public void BindAsync_ResultWithSuccess_ShouldApplyFunctionAndReturnDifferentSuccess()
    {
        var result = Result.Create(5);
        var boundResult = result.Bind(_ => Result.Create("Success"));

        Assert.True(boundResult.IsSuccessful);
        Assert.Equal("Success", boundResult.Value);
    }

    [Fact]
    public async Task Bind_WithSuccessfulResultAndSuccessfulTask_ShouldReturnSuccess()
    {
        var result = Result.Create(10);
        var boundResult = await result.Bind(_ => Task.FromResult(Result.Success));

        Assert.True(boundResult.IsSuccessful);
    }

    [Fact]
    public void Match_WithSuccessfulResult_ShouldExecuteOnSuccessAction()
    {
        var result = Result.Create(5);
        var output = result.Match(
            _ => "Success",
            _ => "Failure");

        Assert.Equal("Success", output);
    }

    [Fact]
    public void Match_WithFailure_ShouldExecuteOnErrorAction()
    {
        var error = Error.Failure();
        Result<int> result = error;
        var output = result.Match(
            _ => "Success",
            _ => "Failure");

        Assert.Equal("Failure", output);
    }

    [Fact]
    public async Task MatchAsync_WithSuccessfulResult_ShouldExecuteOnSuccessFunction()
    {
        var resultTask = Task.FromResult(Result.Create(5));
        var output = await resultTask.Match(
            _ => "Success",
            _ => "Failure");

        Assert.Equal("Success", output);
    }

    [Fact]
    public async Task MatchAsync_WithFailure_ShouldExecuteOnErrorFunction()
    {
        var error = Error.Failure();
        Result<int> result = error;
        var resultTask = Task.FromResult(result);
        var output = await resultTask.Match(
            _ => "Success",
            _ => "Failure");

        Assert.Equal("Failure", output);
    }

    [Fact]
    public void Match_WithSuccess_ShouldInvokeSuccessFunction()
    {
        var result = Result.Success;
        var output = result.Match(
            () => "Success",
            _ => "Failure");

        Assert.Equal("Success", output);
    }

    [Fact]
    public void Match_WithFailure_ShouldInvokeErrorFunction()
    {
        var error = Error.Failure("Error", "Error occurred");
        Result result = error;
        var output = result.Match(
            () => "Success",
            _ => "Failure");

        Assert.Equal("Failure", output);
    }

    [Fact]
    public async Task MatchAsync_WithSuccessfulResult_ShouldInvokeOnSuccessFunction()
    {
        var resultTask = Task.FromResult(Result.Success);
        var matched = await resultTask.Match(
            () => "Operation Succeeded",
            _ => "Operation Failed");

        Assert.Equal("Operation Succeeded", matched);
    }

    [Fact]
    public async Task MatchAsync_WithFailedResult_ShouldInvokeOnErrorFunction()
    {
        var error = Error.Failure("TestError", "A test error occurred");
        Result result = error;
        var resultTask = Task.FromResult(result);
        var matched = await resultTask.Match(
            () => "Operation Succeeded",
            _ => "Operation Failed");

        Assert.Equal("Operation Failed", matched);
    }
}