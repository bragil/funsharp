namespace FunSharp.Tasks;

public static class TaskExtensions
{
    /// <summary>
    /// Intercept the result in case of error.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="errorFunction">Error function</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> OnError<T>(this Task<Result<T>> source, 
                                                   Action<Error> errorFunction)
    {
        var result = await source;
        if (result.HasError)
            errorFunction(result.GetError());

        return result;
    }

    /// <summary>
    /// Intercept the result in case of some value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="valueFunction">Value function</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> OnSomeValue<T>(this Task<Result<T>> source, 
                                                       Action<T> valueFunction)
    {
        var result = await source;
        if (!result.HasError && result.HasValue)
            valueFunction(result.GetValue());

        return result;
    }

    /// <summary>
    /// Intercept the result in case of none value.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="noValueFunction">No value function</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> OnNoneValue<T>(this Task<Result<T>> source, 
                                                       Action noValueFunction)
    {
        var result = await source;
        if (!result.HasError && !result.HasValue)
            noValueFunction();

        return result;
    }

    /////// <summary>
    /////// <![CDATA[
    /////// Chaining a task of Result<T>.
    /////// ]]>
    /////// </summary>
    /////// <typeparam name="TIn">Value type</typeparam>
    /////// <param name="source">Source task</param>
    /////// <param name="function">Function to be applied to the task</param>
    /////// <returns><![CDATA[New task of Result<Unit>]]></returns>
    ////public static async Task<Result<Unit>> Then<TIn>(this Task<Result<TIn>> source,
    ////                                                 Func<TIn, Task> function)
    ////{
    ////    try
    ////    {
    ////        var res = await source;
    ////        if (res.HasError)
    ////            return res.GetError();
            
    ////        await function(res.GetValueOrElse(default));
    ////        return Unit.Create();
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return new Error(ex.Message, ex);
    ////    }
    ////}

    /////// <summary>
    /////// <![CDATA[
    /////// Chaining a task of Result<T>.
    /////// ]]>
    /////// </summary>
    /////// <typeparam name="TIn">Value type</typeparam>
    /////// <param name="source">Source task</param>
    /////// <param name="function">Function to be applied to the task</param>
    /////// <returns><![CDATA[New task of Result<Unit>]]></returns>
    ////public static async Task<Result<Unit>> Then<TIn>(this Task<Result<TIn>> source,
    ////                                                 Action<TIn> function)
    ////{
    ////    try
    ////    {
    ////        var res = await source;
    ////        if (res.HasError)
    ////            return res.GetError();

    ////        function(res.GetValueOrElse(default));
    ////        return Unit.Create();
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return new Error(ex.Message, ex);
    ////    }
    ////}

    /// <summary>
    /// <![CDATA[
    /// Chaining a task of Result<T>.
    /// ]]>
    /// </summary>
    /// <typeparam name="TIn">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="function">Function to be applied to the task</param>
    /// <returns><![CDATA[New task of Result<T>]]></returns>
    public static async Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> source,
                                                           Func<TIn, Task<TOut>> function)
    {
        try
        {
            var res = await source;
            return res.HasError ? res.GetError() : await function(res.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// <![CDATA[
    /// Chaining a task of Result<T>.
    /// ]]>
    /// </summary>
    /// <typeparam name="TIn">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="function">Function to be applied to the task</param>
    /// <returns><![CDATA[New task of Result<T>]]></returns>
    public static async Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> source,
                                                           Func<TIn, TOut> function)
    {
        try
        {
            var res = await source;
            return res.HasError ? res.GetError() : function(res.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// <![CDATA[
    /// Chaining a task of Result<T>.
    /// ]]>
    /// </summary>
    /// <typeparam name="TIn">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="function">Function to be applied to the task</param>
    /// <returns><![CDATA[New task of Result<T>]]></returns>
    public static async Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> source,
                                                           Func<TIn, Task<Result<TOut>>> function)
    {
        try
        {
            var res = await source;
            return res.HasError ? res.GetError() : await function(res.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// <![CDATA[
    /// Chaining a task of Result<T>.
    /// ]]>
    /// </summary>
    /// <typeparam name="TIn">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="function">Function to be applied to the task</param>
    /// <returns><![CDATA[New task of Result<T>]]></returns>
    public static async Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> source,
                                                           Func<TIn, Result<TOut>> function)
    {
        try
        {
            var res = await source;
            return res.HasError ? res.GetError() : function(res.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// Result pattern match
    /// </summary>
    /// <typeparam name="TOut">Return type</typeparam>
    /// <param name="some">Executed when contains value</param>
    /// <param name="none">Executed when not contains value</param>
    /// <param name="error">Executed when has error</param>
    /// <returns>Result of T</returns>
    public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> source,
                                                    Func<TIn, TOut> some, 
                                                    Func<TOut> none, 
                                                    Func<Error, TOut> error)
    {
        return (await source).Match(some, none, error);
    }

    /// <summary>
    /// Fire and forget a task.
    /// </summary>
    /// <param name="task">Task</param>
    /// <param name="errorHandler">Error handler</param>
    public static void FireAndForget(this Task task, Action<Exception> errorHandler = null)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && errorHandler != null)
                errorHandler(t.Exception);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    /// <summary>
    /// Retrying a task {maxRetries} times, with delay.
    /// </summary>
    /// <typeparam name="TResult">Value type</typeparam>
    /// <param name="taskFactory">Function to be executed</param>
    /// <param name="maxRetries">Max of retries</param>
    /// <param name="delay">Delay between retries</param>
    /// <returns><![CDATA[Task<TResult>]]></returns>
    public static async Task<TResult> Retry<TResult>(
                                        this Func<Task<TResult>> taskFactory,
                                        int maxRetries,
                                        TimeSpan delay)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch
            {
                if (i == maxRetries - 1)
                    throw;
                await Task.Delay(delay).ConfigureAwait(false);
            }
        }

        return default; // Should not be reached
    }

    /// <summary>
    /// If the task takes longer than {timeout}, an exception is thrown.
    /// </summary>
    /// <param name="task">The task</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Task</returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task WithTimeout(this Task task, TimeSpan timeout)
    {
        var delayTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(task, delayTask).ConfigureAwait(false);
        if (completedTask == delayTask)
            throw new TimeoutException();

        await task;
    }
}
