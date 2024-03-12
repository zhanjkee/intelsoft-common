namespace Intelsoft.Common.Result;

/// <summary>
///     Provides extension methods for working with results.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Validates the result using the specified validation function and returns a new result.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to validate.</param>
    /// <param name="validate">The validation function.</param>
    /// <param name="errors">Additional errors to include if the validation fails.</param>
    /// <returns>A new result with validation applied.</returns>
    public static Result<T> Validate<T>(
        this Result<T> result,
        Func<T, bool> validate,
        params Error[] errors)
    {
        if (!result.IsSuccessful) return result;

        return validate(result.Value) ? result : errors;
    }

    /// <summary>
    ///     Asynchronously validates the result using the specified validation function and returns a new result.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="sourceTask">The asynchronous result task to validate.</param>
    /// <param name="validate">The validation function.</param>
    /// <param name="errors">Additional errors to include if the validation fails.</param>
    /// <returns>A new result with validation applied.</returns>
    public static async Task<Result<T>> Validate<T>(
        this Task<Result<T>> sourceTask,
        Func<T, bool> validate,
        params Error[] errors)
    {
        var result = await sourceTask.ConfigureAwait(false);
        if (!result.IsSuccessful) return result;

        return validate(result.Value) ? result : errors;
    }

    /// <summary>
    ///     Maps the result value using the specified mapping function and returns a new result.
    /// </summary>
    /// <typeparam name="TIn">The type of the input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result">The result to map.</param>
    /// <param name="mapping">The mapping function.</param>
    /// <returns>A new result with the mapped value.</returns>
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> mapping)
    {
        if (!result.IsSuccessful) return result.Errors;

        return mapping(result.Value);
    }

    /// <summary>
    ///     Asynchronously maps the result value using the specified mapping function and returns a new result.
    /// </summary>
    /// <typeparam name="TIn">The type of the input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="sourceTask">The asynchronous result task to map.</param>
    /// <param name="mapping">The mapping function.</param>
    /// <returns>A new result with the mapped value.</returns>
    public static async Task<Result<TOut>> Map<TIn, TOut>(
        this Task<Result<TIn>> sourceTask,
        Func<TIn, TOut> mapping)
    {
        var result = await sourceTask.ConfigureAwait(false);
        if (!result.IsSuccessful) return result.Errors;

        return mapping(result.Value);
    }

    /// <summary>
    ///     Binds the result value using the specified binding function and returns a new result.
    /// </summary>
    /// <typeparam name="TIn">The type of the input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result">The result to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>A new result with the bound value.</returns>
    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> binder)
    {
        return result.IsSuccessful ? binder(result.Value) : result.Errors;
    }

    /// <summary>
    ///     Binds the specified binder function to the result if the result is successful.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value contained in the result.</typeparam>
    /// <param name="result">The result to which the binder function is applied.</param>
    /// <param name="binder">The function that is applied to the result value if the result is successful.</param>
    /// <returns>
    ///     Returns a new <see cref="Result" /> instance. If the original result is successful, the binder function is applied,
    ///     and its result is returned. If the original result is unsuccessful, its errors are returned in a new result
    ///     instance.
    /// </returns>
    public static Result Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Result> binder)
    {
        return result.IsSuccessful ? binder(result.Value) : result.Errors;
    }

    /// <summary>
    ///     Asynchronously binds the result value using the specified binding function and returns a new result.
    /// </summary>
    /// <typeparam name="TIn">The type of the input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result">The result to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>A new result with the bound value.</returns>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Task<Result<TIn>> result,
        Func<TIn, Task<Result<TOut>>> binder)
    {
        var awaitedResult = await result.ConfigureAwait(false);
        if (!awaitedResult.IsSuccessful) return awaitedResult.Errors;

        return await binder(awaitedResult.Value).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously binds the specified asynchronous binder function to the result if the result is successful.
    /// </summary>
    /// <typeparam name="TIn">The type of the input value contained in the result.</typeparam>
    /// <param name="result">The result to which the asynchronous binder function is applied.</param>
    /// <param name="binder">The asynchronous function that is applied to the result value if the result is successful.</param>
    /// <returns>
    ///     Returns a <see cref="Task{Result}" /> that represents the asynchronous operation.
    ///     If the original result is successful, the asynchronous binder function is applied, and its result is awaited and
    ///     returned.
    ///     If the original result is unsuccessful, a completed task containing the original result is returned.
    /// </returns>
    /// <remarks>
    ///     It's important to note that if the original result is unsuccessful, the binder function will not be executed,
    ///     and the original errors are returned immediately, wrapped in a completed task.
    /// </remarks>
    public static async Task<Result> Bind<TIn>(
        this Result<TIn> result,
        Func<TIn, Task<Result>> binder)
    {
        // Ensure the binder function is only called if the result is successful.
        if (!result.IsSuccessful)
            // Return the original unsuccessful result wrapped in a Task.
            return result.Errors;

        // Execute the binder function and return its result.
        return await binder(result.Value);
    }

    /// <summary>
    ///     Matches the result and executes the appropriate function based on the result status.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to match.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onError">The function to execute if the result has errors.</param>
    /// <returns>The result of executing the appropriate function.</returns>
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<IReadOnlyCollection<Error>, T> onError)
    {
        return result.IsSuccessful ? onSuccess() : onError(result.Errors);
    }

    /// <summary>
    ///     Asynchronously matches the result and executes the appropriate function based on the result status.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="sourceTask">The asynchronous result task to match.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onError">The function to execute if the result has errors.</param>
    /// <returns>The result of executing the appropriate function.</returns>
    public static async Task<T> Match<T>(
        this Task<Result> sourceTask,
        Func<T> onSuccess,
        Func<IReadOnlyCollection<Error>, T> onError)
    {
        var result = await sourceTask.ConfigureAwait(false);
        return result.IsSuccessful ? onSuccess() : onError(result.Errors);
    }

    /// <summary>
    ///     Matches the result and executes the appropriate function based on the result status.
    /// </summary>
    /// <typeparam name="TIn">The type of the input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="result">The result to match.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onError">The function to execute if the result has errors.</param>
    /// <returns>The result of executing the appropriate function.</returns>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<IReadOnlyCollection<Error>, TOut> onError)
    {
        return result.IsSuccessful ? onSuccess(result.Value) : onError(result.Errors);
    }

    /// <summary>
    ///     Asynchronously matches the result and executes the appropriate function based on the result status.
    /// </summary>
    /// <typeparam name="TIn">The type of the input result value.</typeparam>
    /// <typeparam name="TOut">The type of the output result value.</typeparam>
    /// <param name="sourceTask">The asynchronous result task to match.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onError">The function to execute if the result has errors.</param>
    /// <returns>The result of executing the appropriate function.</returns>
    public static async Task<TOut> Match<TIn, TOut>(
        this Task<Result<TIn>> sourceTask,
        Func<TIn, TOut> onSuccess,
        Func<IReadOnlyCollection<Error>, TOut> onError)
    {
        var result = await sourceTask.ConfigureAwait(false);
        return result.IsSuccessful ? onSuccess(result.Value) : onError(result.Errors);
    }
}