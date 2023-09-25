using FunSharp;
using FunSharp.Nullables;
using Shouldly;

namespace Tests;

public class NullableTests
{
    [Test]
    public void Should_convert_Nullable_to_Maybe()
    {
        int? nullableWithValue = 100;
        var maybeWithValue = nullableWithValue.ToMaybe();
        maybeWithValue.HasValue.ShouldBeTrue();

        int? nullableWithNull = null;
        var maybeWithoutValue = nullableWithNull.ToMaybe();
        maybeWithoutValue.HasValue.ShouldBeFalse();
    }

    [Test]
    public void Should_convert_Nullable_to_Result()
    {
        int? nullableWithValue = 100;
        var resultWithValue = nullableWithValue.ToResult();
        resultWithValue.HasValue.ShouldBeTrue();
        resultWithValue.HasError.ShouldBeFalse();

        int? nullableWithNull = null;
        var resultWithoutValue = nullableWithNull.ToResult();
        resultWithoutValue.HasValue.ShouldBeFalse();
        resultWithoutValue.HasError.ShouldBeFalse();
    }

    [Test]
    public void Should_match_to_correct_value()
    {
        int? nullableWithValue = 100;
        var value = nullableWithValue.Match(some: v => v, none: () => 0);
        value.ShouldBe(100);

        int? nullableWithNull = null;
        var zero = nullableWithNull.Match(some: v => v, none: () => 0);
        zero.ShouldBe(0);
    }

    [Test]
    public void Should_chaining_with_Then()
    {
        int? nullableWithValue = 100;
        var maybeWithValue = nullableWithValue.Then(n => 100 * 2);
        maybeWithValue.HasValue.ShouldBeTrue();
        maybeWithValue.GetValueOrElse(0).ShouldBe(200);

        int? nullableWithNull = null;
        var maybeWithoutValue = nullableWithNull.Then(n => n);
        maybeWithoutValue.HasValue.ShouldBeFalse();
    }

    [Test]
    public void Should_chaining_with_Then_and_Maybe()
    {
        Maybe<int> maybe = 50;
        int? nullableWithValue = 100;
        var maybeWithValue = nullableWithValue.Then(n => maybe);
        maybeWithValue.HasValue.ShouldBeTrue();
        maybeWithValue.GetValueOrElse(0).ShouldBe(50);

        Maybe<int> defaultMaybe = default;
        int? nullableWithNull = null;
        var maybeWithoutValue = nullableWithNull.Then(n => defaultMaybe);
        maybeWithoutValue.HasValue.ShouldBeFalse();
    }
}
