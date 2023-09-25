namespace FunSharp.Nullables;

public static class NullableExtensions
{
    public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct
        => nullable ?? default;

    public static Result<T> ToResult<T>(this T? nullable) where T : struct
        => nullable ?? default;

    public static Maybe<TOut> Then<TIn, TOut>(this TIn? nullable, Func<TIn, TOut> function)
        where TIn : struct
        => nullable.HasValue ? function(nullable.Value) : default;

    public static Maybe<TOut> Then<TIn, TOut>(this TIn? nullable, Func<TIn, Maybe<TOut>> function)
        where TIn : struct
        => nullable.HasValue ? function(nullable.Value) : default;

    public static TOut Match<TIn, TOut>(this TIn? nullable,
            Func<TIn, TOut> some,
            Func<TOut> none = null)
            where TIn : struct
        => nullable.HasValue ? some(nullable.Value) : none();
}
