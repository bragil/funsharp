namespace FunSharp;

/// <summary>
/// Functional utilities.
/// </summary>
public static class FnUtils
{
    internal static bool HasValue<T>(T value)
        => value is Unit || !EqualityComparer<T>.Default.Equals(value, default);

    internal static bool HasError(Error error)
        => !EqualityComparer<Error>.Default.Equals(error, default);

    #region Try

    /////// <summary>
    /////// Functional replace for try..catch block.
    /////// </summary>
    /////// <param name="function">Function to execute</param>
    /////// <param name="errorHandler">Error handler (optional)</param>
    /////// <returns><![CDATA[Result<Unit>]]></returns>
    ////public static Result<Unit> Try(Func<Unit> function, Action<Error> errorHandler = null)
    ////{
    ////    try
    ////    {
    ////        return function();
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return HandleError(ex, errorHandler);
    ////    }
    ////}

    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<T>]]></returns>
    public static Result<T> Try<T>(Func<T> function, Action<Error> errorHandler = null)
    {
        try
        {
            return function();
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<T>]]></returns>
    public static Result<T> Try<T>(Func<Result<T>> function, Action<Error> errorHandler = null)
    {
        try
        {
            return function();
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    /////// <summary>
    /////// Functional replace for try..catch block.
    /////// </summary>
    /////// <typeparam name="TIn">Value type</typeparam>
    /////// <param name="value">The value</param>
    /////// <param name="function">Function to execute</param>
    /////// <param name="errorHandler">Error handler (optional)</param>
    /////// <returns><![CDATA[Result<Unit>]]></returns>
    ////public static Result<Unit> Try<TIn>(TIn value, Func<TIn, Unit> function,
    ////                                    Action<Error> errorHandler = null)
    ////{
    ////    try
    ////    {
    ////        return function(value);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return HandleError(ex, errorHandler);
    ////    }
    ////}

    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <typeparam name="TIn">Value type</typeparam>
    /// <param name="value">The value</param>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<TOut>]]></returns>
    public static Result<TOut> Try<TIn, TOut>(TIn value, Func<TIn, TOut> function,
                                              Action<Error> errorHandler = null)
    {
        try
        {
            return function(value);
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <typeparam name="TIn">Value type (in)</typeparam>
    /// <typeparam name="TOut">Value type (out)</typeparam>
    /// <param name="value">The value</param>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<TOut>]]></returns>
    public static Result<TOut> Try<TIn, TOut>(TIn value, Func<TIn, Result<TOut>> function,
                                              Action<Error> errorHandler = null)
    {
        try
        {
            return function(value);
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    #endregion

    #region TryAsync

    /////// <summary>
    /////// Functional replace for try..catch block (async version).
    /////// </summary>
    /////// <param name="function">Function to execute</param>
    /////// <param name="errorHandler">Error handler (optional)</param>
    /////// <returns><![CDATA[Task<Result<Unit>>]]></returns>
    ////public async static Task<Result<Unit>> TryAsync(
    ////                                            Func<Task<Unit>> function,
    ////                                            Action<Error> errorHandler = null)
    ////{
    ////    try
    ////    {
    ////        return await function();
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return HandleError(ex, errorHandler);
    ////    }
    ////}

    /// <summary>
    /// Functional replace for try..catch block (async version).
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> TryAsync<T>(
                                            Func<Task<T>> function, 
                                            Action<Error> errorHandler = null)
    {
        try
        {
            return await function();
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    /// <summary>
    /// Functional replace for try..catch block (async version).
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> TryAsync<T>(
                                            Func<Task<Result<T>>> function,
                                            Action<Error> errorHandler = null)
    {
        try
        {
            return await function();
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    /////// <summary>
    /////// Functional replace for try..catch block (async version).
    /////// </summary>
    /////// <typeparam name="TIn">Value type</typeparam>
    /////// <param name="value">The value</param>
    /////// <param name="function">Function to execute</param>
    /////// <param name="errorHandler">Error handler (optional)</param>
    /////// <returns><![CDATA[Task<Result<Unit>>]]></returns>
    ////public static async Task<Result<Unit>> TryAsync<TIn>(
    ////                                            TIn value, 
    ////                                            Func<TIn, Task<Unit>> function,
    ////                                            Action<Error> errorHandler = null)
    ////{
    ////    try
    ////    {
    ////        return await function(value);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return HandleError(ex, errorHandler);
    ////    }
    ////}

    /// <summary>
    /// Functional replace for try..catch block (async version).
    /// </summary>
    /// <typeparam name="TIn">Value type (in)</typeparam>
    /// <typeparam name="TOut">Value type (out)</typeparam>
    /// <param name="value">The value</param>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Task<Result<TOut>>]]></returns>
    public static async Task<Result<TOut>> TryAsync<TIn, TOut>(
                                                TIn value, 
                                                Func<TIn, Task<TOut>> function,
                                                Action<Error> errorHandler = null)
    {
        try
        {
            return await function(value);
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    /// <summary>
    /// Functional replace for try..catch block (async version).
    /// </summary>
    /// <typeparam name="TIn">Value type (in)</typeparam>
    /// <typeparam name="TOut">Value type (out)</typeparam>
    /// <param name="value">The value</param>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Task<Result<TOut>>]]></returns>
    public static async Task<Result<TOut>> TryAsync<TIn, TOut>(
                                                TIn value, 
                                                Func<TIn, Task<Result<TOut>>> function,
                                                Action<Error> errorHandler = null)
    {
        try
        {
            return await function(value);
        }
        catch (Exception ex)
        {
            return HandleError(ex, errorHandler);
        }
    }

    #endregion

    #region Using - All synchronous

    /////// <summary>
    /////// Funcional replace for using block.
    /////// </summary>
    /////// <param name="disposable">Function that returns IDisposable</param>
    /////// <param name="function">Function to execute inside using block</param>
    /////// <param name="errorHandler">Error handler</param>
    /////// <returns><![CDATA[Result<Unit>]]></returns>
    ////public static Result<Unit> Using<TDisp>(
    ////                            Func<TDisp> disposable, 
    ////                            Func<TDisp, Unit> function,
    ////                            Action<Error> errorHandler = null) where TDisp : IDisposable
    ////    => Try(() =>
    ////    {
    ////        using var d = disposable();
    ////        return function(d);
    ////    }, 
    ////    errorHandler);

    /// <summary>
    /// Funcional replace for using block.
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Result<T>]]></returns>
    public static Result<T> Using<T, TDisp>(
                                Func<TDisp> disposable, 
                                Func<TDisp, T> function,
                                Action<Error> errorHandler = null) where TDisp : IDisposable
        => Try(() =>
        {
            using var d = disposable();
            return function(d);
        },
        errorHandler);

    /// <summary>
    /// Funcional replace for using block.
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Result<T>]]></returns>
    public static Result<T> Using<T, TDisp>(
                                Func<TDisp> disposable, 
                                Func<TDisp, Result<T>> function,
                                Action<Error> errorHandler = null) where TDisp : IDisposable
        => Try(() =>
        {
            using var d = disposable();
            return function(d);
        },
        errorHandler);

    #endregion

    #region Using - async function, sync IDisposable

    /////// <summary>
    /////// Funcional replace for using block (async version).
    /////// </summary>
    /////// <param name="disposable">Function that returns IDisposable</param>
    /////// <param name="function">Function to execute inside using block</param>
    /////// <param name="errorHandler">Error handler</param>
    /////// <returns><![CDATA[Task<Result<Unit>>]]></returns>
    ////public async static Task<Result<Unit>> Using<TDisp>(
    ////                                        Func<TDisp> disposable, 
    ////                                        Func<TDisp, Task<Unit>> function,
    ////                                        Action<Error> errorHandler = null) where TDisp : IDisposable
    ////    => await TryAsync(async () =>
    ////    {
    ////        using var d = disposable();
    ////        return await function(d);
    ////    },
    ////    errorHandler);

    /// <summary>
    /// Funcional replace for using block (async version).
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> Using<T, TDisp>(
                                            Func<TDisp> disposable, 
                                            Func<TDisp, Task<T>> function,
                                            Action<Error> errorHandler = null) where TDisp : IDisposable
        => await TryAsync(async () =>
        {
            using var d = disposable();
            return await function(d);
        },
        errorHandler);

    /// <summary>
    /// Funcional replace for using block (async version).
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> Using<T, TDisp>(
                                            Func<TDisp> disposable, 
                                            Func<TDisp, Task<Result<T>>> function,
                                            Action<Error> errorHandler = null) where TDisp : IDisposable
        => await TryAsync(async () =>
        {
            using var d = disposable();
            return await function(d);
        },
        errorHandler);

    #endregion

    #region Using - async function, async IDisposable

    /////// <summary>
    /////// Funcional replace for using block (async version).
    /////// </summary>
    /////// <param name="disposable">Function that returns IDisposable</param>
    /////// <param name="function">Function to execute inside using block</param>
    /////// <param name="errorHandler">Error handler</param>
    /////// <returns><![CDATA[Task<Result<Unit>>]]></returns>
    ////public async static Task<Result<Unit>> Using<TDisp>(
    ////                                        Func<Task<TDisp>> disposable,
    ////                                        Func<TDisp, Task<Unit>> function,
    ////                                        Action<Error> errorHandler = null) where TDisp : IDisposable
    ////    => await TryAsync(async () =>
    ////    {
    ////        using var d = await disposable();
    ////        return await function(d);
    ////    },
    ////    errorHandler);

    /// <summary>
    /// Funcional replace for using block (async version).
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> Using<T, TDisp>(
                                            Func<Task<TDisp>> disposable,
                                            Func<TDisp, Task<T>> function,
                                            Action<Error> errorHandler = null) where TDisp : IDisposable
        => await TryAsync(async () =>
        {
            using var d = await disposable();
            return await function(d);
        },
        errorHandler);

    /// <summary>
    /// Funcional replace for using block (async version).
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> Using<T, TDisp>(
                                            Func<Task<TDisp>> disposable,
                                            Func<TDisp, Task<Result<T>>> function,
                                            Action<Error> errorHandler = null) where TDisp : IDisposable
        => await TryAsync(async () =>
        {
            using var d = await disposable();
            return await function(d);
        },
        errorHandler);

    #endregion

    #region Using - sync function, async IDisposable

    /////// <summary>
    /////// Funcional replace for using block (async version).
    /////// </summary>
    /////// <param name="disposable">Function that returns IDisposable</param>
    /////// <param name="function">Function to execute inside using block</param>
    /////// <param name="errorHandler">Error handler</param>
    /////// <returns><![CDATA[Task<Result<Unit>>]]></returns>
    ////public async static Task<Result<Unit>> Using<TDisp>(
    ////                                        Func<Task<TDisp>> disposable,
    ////                                        Func<TDisp, Unit> function,
    ////                                        Action<Error> errorHandler = null) where TDisp : IDisposable
    ////    => await TryAsync(async () =>
    ////    {
    ////        using var d = await disposable();
    ////        return function(d);
    ////    },
    ////    errorHandler);

    /// <summary>
    /// Funcional replace for using block (async version).
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> Using<T, TDisp>(
                                            Func<Task<TDisp>> disposable,
                                            Func<TDisp, T> function,
                                            Action<Error> errorHandler = null) where TDisp : IDisposable
        => await TryAsync(async () =>
        {
            using var d = await disposable();
            return function(d);
        },
        errorHandler);

    /// <summary>
    /// Funcional replace for using block (async version).
    /// </summary>
    /// <param name="disposable">Function that returns IDisposable</param>
    /// <param name="function">Function to execute inside using block</param>
    /// <param name="errorHandler">Error handler</param>
    /// <returns><![CDATA[Task<Result<T>>]]></returns>
    public async static Task<Result<T>> Using<T, TDisp>(
                                            Func<Task<TDisp>> disposable,
                                            Func<TDisp, Result<T>> function,
                                            Action<Error> errorHandler = null) where TDisp : IDisposable
        => await TryAsync(async () =>
        {
            using var d = await disposable();
            return function(d);
        },
        errorHandler);

    #endregion

    internal static Error HandleError(Exception ex, Action<Error> errorHandler = null)
    {
        var error = new Error(ex.Message, ex);
        errorHandler?.Invoke(error);

        return error;
    }
}