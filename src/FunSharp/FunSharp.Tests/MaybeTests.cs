using FunSharp;
using Shouldly;

namespace Tests;

public class MaybeTests
{
    [Test]
    public void Should_HasValue_be_false()
    {
        string nullValue = null;
        Maybe<string> maybe = nullValue;
        maybe.HasValue.ShouldBeFalse();
    }

    [Test]
    public void Should_HasValue_be_true()
    {
        string value = "test";
        Maybe<string> maybe = value;

        maybe.HasValue.ShouldBeTrue();
    }

    [Test]
    public void Should_change_value_after_Then()
    {
        Maybe<string> maybe = "test";
        var novo = maybe.Then(s => s.ToUpper()).GetValueOrElse("");

        novo.ShouldBe("TEST");
    }

    [Test]
    public void Should_change_value_after_Then_with_Maybe()
    {
        Maybe<int> intMaybe = 99;
        Maybe<string> maybe = "teste";
        var novo = maybe
                    .Then(s => intMaybe)
                    .GetValueOrElse(0);

        novo.ShouldBe(99);
    }

    [Test]
    public void Should_do_correct_pattern_matching()
    {
        string valorNulo = null;
        Maybe<string> maybe1 = valorNulo;
        var result = maybe1.Match(some: s => s, none: () => "none");
        result.ShouldBe("none");

        string valor = "teste";
        Maybe<string> maybe2 = valor;
        var result2 = maybe2.Match(some: s => s, none: () => "none");
        result2.ShouldBe("teste");
    }
}
