namespace Intelsoft.Common.Result;

/// <summary>
///     Represents an error with a specific code, description, and type.
/// </summary>
public struct Error
{
    /// <summary>
    ///     Gets the error code.
    /// </summary>
    public readonly string Code;

    /// <summary>
    ///     Gets the error description.
    /// </summary>
    public readonly string Description;

    /// <summary>
    ///     Gets the type of the error.
    /// </summary>
    public readonly ErrorType ErrorType;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> struct.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="errorType">The type of the error.</param>
    private Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> struct.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="type">The type of the error.</param>
    /// <returns>A new instance of the <see cref="Error" /> struct.</returns>
    public static Error Create(string code, string description, ErrorType type)
    {
        return new Error(code, description, type);
    }

    /// <summary>
    ///     Creates a failure error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.Failure".</param>
    /// <param name="description">The error description. Default is "A failure has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing a failure error.</returns>
    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occurred.")
    {
        return new Error(code, description, ErrorType.Failure);
    }

    /// <summary>
    ///     Creates a validation error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.Validation".</param>
    /// <param name="description">The error description. Default is "A validation error has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing a validation error.</returns>
    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred.")
    {
        return new Error(code, description, ErrorType.Validation);
    }

    /// <summary>
    ///     Creates an unauthorized error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.Unauthorized".</param>
    /// <param name="description">The error description. Default is "An 'Unauthorized' error has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing an unauthorized error.</returns>
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.")
    {
        return new Error(code, description, ErrorType.Unauthorized);
    }

    /// <summary>
    ///     Creates a forbidden error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.Forbidden".</param>
    /// <param name="description">The error description. Default is "A 'Forbidden' error has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing a forbidden error.</returns>
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A 'Forbidden' error has occurred.")
    {
        return new Error(code, description, ErrorType.Forbidden);
    }

    /// <summary>
    ///     Creates a 'Not Found' error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.NotFound".</param>
    /// <param name="description">The error description. Default is "A 'Not Found' error has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing a 'Not Found' error.</returns>
    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.")
    {
        return new Error(code, description, ErrorType.NotFound);
    }

    /// <summary>
    ///     Creates a conflict error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.Conflict".</param>
    /// <param name="description">The error description. Default is "A conflict error has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing a conflict error.</returns>
    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.")
    {
        return new Error(code, description, ErrorType.Conflict);
    }

    /// <summary>
    ///     Creates an unexpected error with the specified code and description.
    /// </summary>
    /// <param name="code">The error code. Default is "General.Unexpected".</param>
    /// <param name="description">The error description. Default is "An unexpected error has occurred.".</param>
    /// <returns>A new instance of the <see cref="Error" /> struct representing an unexpected error.</returns>
    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.")
    {
        return new Error(code, description, ErrorType.Unexpected);
    }
}