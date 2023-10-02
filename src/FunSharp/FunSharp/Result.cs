using static FunSharp.FnUtils;

namespace FunSharp;

/// <summary>
/// Result Monad.
/// </summary>
/// <typeparam name="TValue">Value type</typeparam>
public readonly struct Result<TValue>
{
    private readonly Maybe<TValue> value;
    private readonly Error error;
    public readonly bool HasValue => value.HasValue;
    public readonly bool HasError => HasError(error);

    internal Result(TValue value)
    {
        this.value = value;
        error = default;
    }

    internal Result(Error error)
    {
        value = Maybe.Empty<TValue>();
        this.error = error;
    }

    internal Result(None _)
    {
        value = Maybe.Empty<TValue>();
        error = default;
    }

    internal TValue GetValue()
        => value.GetValue();

    internal Error GetError()
        => error;

    /// <summary>
    /// Intercept the result in case of error.
    /// </summary>
    /// <param name="errorFunction">Error function</param>
    /// <returns><![CDATA[Result<TValue>]]></returns>
    public Result<TValue> OnError(Action<Error> errorFunction)
    {
        if (HasError)
            errorFunction(error);

        return this;
    }

    /// <summary>
    /// Intercept the result in case of some value.
    /// </summary>
    /// <param name="valueFunction">Value function</param>
    /// <returns><![CDATA[Result<TValue>]]></returns>
    public Result<TValue> OnSomeValue(Action<TValue> valueFunction)
    {
        if (!HasError && HasValue)
            valueFunction(value.GetValue());

        return this;
    }

    /// <summary>
    /// Intercept the result in case of none value.
    /// </summary>
    /// <param name="noValueFunction">Value function</param>
    /// <returns><![CDATA[Result<TValue>]]></returns>
    public Result<TValue> OnNoneValue(Action noValueFunction)
    {
        if (!HasError && !HasValue)
            noValueFunction();

        return this;
    }

    /// <summary>
    /// Return the value or the fallback (if not value).
    /// </summary>
    /// <param name="fallback">Value to be returned if no value</param>
    /// <returns>Value or fallback</returns>
    public TValue GetValueOrElse(TValue fallback)
        => HasError ? fallback : value.GetValueOrElse(fallback);

    /// <summary>
    /// Result pattern match
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="some">Executed when contains value</param>
    /// <param name="none">Executed when not contains value</param>
    /// <param name="error">Executed when has error</param>
    /// <returns>Result of T</returns>
    public T Match<T>(Func<TValue, T> some, Func<T> none, Func<Error, T> error)
        => HasError ? error(this.error) : value.Match(some, none);

    /// <summary>
    /// Result pattern match
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="success">Executed when success</param>
    /// <param name="error">Executed when has error</param>
    /// <returns>Result of T</returns>
    public T Match<T>(Func<Unit, T> success, Func<Error, T> error)
        => HasError ? error(this.error) : success(Unit.Create());


    public Result<T> Then<T>(Func<TValue, T> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => Try(GetValue(), function),
            (false, false) => None.Create()
        };

    public Result<T> Then<T>(Func<TValue, Result<T>> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => Try(GetValue(), function),
            (false, false) => None.Create()
        };

    ////public Result<Unit> Then(Func<TValue, Unit> function)
    ////    => (HasError, HasValue) switch
    ////    {
    ////        (true, _) => error,
    ////        (false, true) => Try(GetValue(), function),
    ////        (false, false) => None.Create()
    ////    };

    public async Task<Result<T>> Then<T>(Func<TValue, Task<T>> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => await TryAsync(GetValue(), function),
            (false, false) => None.Create()
        };

    public async Task<Result<T>> Then<T>(Func<TValue, Task<Result<T>>> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => await TryAsync(GetValue(), function),
            (false, false) => None.Create()
        };

    ////public async Task<Result<Unit>> Then(Func<TValue, Task<Unit>> function)
    ////    => (HasError, HasValue) switch
    ////    {
    ////        (true, _) => error,
    ////        (false, true) => await TryAsync(GetValue(), function),
    ////        (false, false) => None.Create()
    ////    };

    /// <summary>
    /// Implicit cast operator for return with value.
    /// </summary>
    /// <param name="theValue">Value</param>
    public static implicit operator Result<TValue>(TValue theValue)
        => new(theValue);

    /// <summary>
    /// Implicit cast operator for error.
    /// </summary>
    /// <param name="error">Error</param>
    public static implicit operator Result<TValue>(Error error)
        => new(error);

    /// <summary>
    /// Operador de cast implícito para None (nenhum valor).
    /// </summary>
    /// <param name="none">Objeto None</param>
    public static implicit operator Result<TValue>(None none)
        => new(none);

    /// <summary>
    /// Operador de cast implícito para Opt[TValue].
    /// </summary>
    /// <param name="maybe">Objeto Opt[TValue]</param>
    public static implicit operator Result<TValue>(Maybe<TValue> maybe)
        => maybe.HasValue 
            ? new Result<TValue>(maybe.GetValue()) 
            : new Result<TValue>(None.Create());
}

public static class Result
{
    public static Result<T> Of<T>(T value)
        => value;

    public static Result<T> Of<T>(T? nullable) where T : struct
        => nullable ?? default;

    public static Result<Unit> Then(Func<Unit> function)
        => Try(function);

    public static Result<T> Then<T>(Func<T> function)
        => Try(function);

    public static Result<T> Then<T>(Func<Result<T>> function)
        => Try(function);

    public async static Task<Result<Unit>> Then(Func<Task<Unit>> function)
        => await TryAsync(function);

    public async static Task<Result<T>> Then<T>(Func<Task<T>> function)
        => await TryAsync(function);

    public async static Task<Result<T>> Then<T>(Func<Task<Result<T>>> function)
        => await TryAsync(function);

    public static Result<C> SelectMany<A, B, C>(this Result<A> monad,
                                                 Func<A, Result<B>> function,
                                                 Func<A, B, C> projection)
            => monad.Then(outer => function(outer)
                    .Then(inner => projection(outer, inner)));

    public static Result<A> Select<A, B, C>(this Result<C> first, Func<C, A> map)
        => first.Then(map);


    public static Result<TOut> Select<TIn, TOut>(this Result<TIn> ma, Func<TIn, TOut> f)
    {
        if (ma.HasError)
            return ma.GetError();

        return Try(ma.GetValueOrElse(default), f);
    }

    public static Result<TOut> SelectMany<TIn, TOut>(this Result<TIn> ma, Func<TIn, Result<TOut>> f)
    {
        if (ma.HasError)
            return ma.GetError();

        return Try(ma.GetValueOrElse(default), f);
    }
}