namespace Intelsoft.Common.Result;

/// <summary>
///     Represents the type of an error.
/// </summary>
public enum ErrorType
{
    /// <summary>
    ///     Indicates a failure error type.
    /// </summary>
    Failure,

    /// <summary>
    ///     Indicates a validation error type.
    /// </summary>
    Validation,

    /// <summary>
    ///     Indicates an unauthorized error type.
    /// </summary>
    Unauthorized,

    /// <summary>
    ///     Indicates a forbidden error type.
    /// </summary>
    Forbidden,

    /// <summary>
    ///     Indicates a 'Not Found' error type.
    /// </summary>
    NotFound,

    /// <summary>
    ///     Indicates a conflict error type.
    /// </summary>
    Conflict,

    /// <summary>
    ///     Indicates an unexpected error type.
    /// </summary>
    Unexpected
}