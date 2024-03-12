namespace Intelsoft.Common.Result;

/// <summary>
///     Represents the result of an operation.
/// </summary>
public interface IResult
{
    /// <summary>
    ///     Gets a value indicating whether the operation was successful.
    /// </summary>
    bool IsSuccessful { get; }

    /// <summary>
    ///     Gets a list of errors associated with the operation.
    /// </summary>
    Error[] Errors { get; }
}

/// <summary>
///     Represents the result of an operation without a specific result value.
/// </summary>
public readonly struct Result : IResult
{
    /// <summary>
    ///     Gets a result representing a successful operation with no errors.
    /// </summary>
    public static readonly Result Success = new(true);

    private Result(bool isSuccessful, params Error[] errors)
    {
        IsSuccessful = isSuccessful;
        Errors = errors;
    }

    /// <inheritdoc />
    public bool IsSuccessful { get; }

    /// <inheritdoc />
    public Error[] Errors { get; } = Array.Empty<Error>();

    /// <summary>
    ///     Gets the first error from a collection of results or returns a success result if all are successful.
    /// </summary>
    /// <param name="results">The collection of results to check.</param>
    /// <returns>The first error encountered or a success result if all are successful.</returns>
    public static Result GetFirstErrorOrSuccess(params IResult[] results)
    {
        foreach (var result in results)
            if (!result.IsSuccessful)
                return result.Errors;

        return Success;
    }

    /// <summary>
    ///     Creates a result with a successful status and a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The result value.</param>
    /// <returns>A new instance of <see cref="Result{T}" />.</returns>
    public static Result<T> Create<T>(T value)
    {
        return new Result<T>(true, value);
    }

    /// <summary>
    ///     Implicitly converts an <see cref="Error" /> to a failed result with the error.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A new instance of <see cref="Result" />.</returns>
    public static implicit operator Result(Error error)
    {
        return new Result(false, error);
    }

    /// <summary>
    ///     Implicitly converts an array of errors to a failed result with the errors.
    /// </summary>
    /// <param name="errors">The array of errors to convert.</param>
    /// <returns>A new instance of <see cref="Result" />.</returns>
    public static implicit operator Result(Error[] errors)
    {
        return new Result(false, errors);
    }
}

/// <summary>
///     Represents the result of an operation with a specific result value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public readonly struct Result<T> : IResult
{
    internal Result(bool isSuccessful, T value, params Error[] errors)
    {
        IsSuccessful = isSuccessful;
        Value = value;
        Errors = errors;
    }

    /// <summary>
    ///     Gets the result value.
    /// </summary>
    public T Value { get; }

    /// <inheritdoc />
    public bool IsSuccessful { get; }

    /// <inheritdoc />
    public Error[] Errors { get; } = Array.Empty<Error>();

    /// <summary>
    ///     Implicitly converts a value to a successful result with the value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A new instance of <see cref="Result{T}" />.</returns>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(true, value);
    }

    /// <summary>
    ///     Implicitly converts an <see cref="Error" /> to a failed result with the error.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A new instance of <see cref="Result{T}" />.</returns>
    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(false, default!, error);
    }

    /// <summary>
    ///     Implicitly converts an array of errors to a failed result with the errors.
    /// </summary>
    /// <param name="errors">The array of errors to convert.</param>
    /// <returns>A new instance of <see cref="Result{T}" />.</returns>
    public static implicit operator Result<T>(Error[] errors)
    {
        return new Result<T>(false, default!, errors);
    }
}