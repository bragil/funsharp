using System;

namespace FunSharp;

/// <summary>
/// Maybe Monad
/// </summary>
/// <typeparam name="TValue">Value type</typeparam>
public readonly struct Maybe<TValue>
{
    private readonly TValue value;
    public readonly bool HasValue => FnUtils.HasValue(value);

    internal Maybe(TValue value)
        => this.value = value;

    internal Maybe(None _)
        => value = default;

    internal TValue GetValue() => value;

    public TValue GetValueOrElse(TValue fallback)
            => HasValue ? value : fallback;

    public T Match<T>(Func<TValue, T> some, Func<T> none)
        => HasValue ? some(value) : none();

    /// <summary>
    /// Provides execution chaining.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="function">Function to be applied to the value</param>
    /// <returns>Result with new value</returns>
    public Maybe<T> Then<T>(Func<TValue, T> function)
        => HasValue ? function(value) : default;

    /// <summary>
    /// Provides execution chaining.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="function">Function to be applied to the value</param>
    /// <returns>Result with new value</returns>
    public Maybe<T> Then<T>(Func<TValue, Maybe<T>> function)
            => HasValue ? function(value) : default;

    /// <summary>
    /// Provides execution chaining.
    /// </summary>
    /// <param name="function">Function to be applied to the value</param>
    /// <returns>Unit</returns>
    public Maybe<Unit> Then(Action<TValue> function)
            => HasValue ? Execute(function) : default;

    private Unit Execute(Action<TValue> function)
    {
        function(value);
        return Unit.Create();
    }


    public static implicit operator Maybe<TValue>(TValue value)
            => new(value);

    public static implicit operator Maybe<TValue>(None none)
        => new(none);
}

public static class Maybe
{
    public static Maybe<T> Of<T>(T value) 
        => new(value);

    public static Maybe<T> Of<T>(T? nullable) where T : struct
        => nullable ?? default;

    public static Maybe<T> Empty<T>()
        => new(None.Create());
}

public static class MaybeExtensions
{
    public static Maybe<C> SelectMany<A, B, C>(this Maybe<A> monad,
                                                 Func<A, Maybe<B>> function,
                                                 Func<A, B, C> projection)
            => monad.Then(outer => function(outer)
                    .Then(inner => projection(outer, inner)));

    public static Maybe<A> Select<A, B, C>(this Maybe<C> first, Func<C, A> map)
        => first.Then(map);


    public static Maybe<TOut> Select<TIn, TOut>(this Maybe<TIn> ma, Func<TIn, TOut> f)
    {
        return f(ma.GetValueOrElse(default));
    }

    public static Maybe<TOut> SelectMany<TIn, TOut>(this Maybe<TIn> ma, Func<TIn, Maybe<TOut>> f)
    {
        return f(ma.GetValueOrElse(default));
    }
}