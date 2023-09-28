using FunSharp;
using FunSharp.Tasks;
using Shouldly;

namespace Tests;

public class ResultTests
{
    [Test]
    public void Should_HasValue_be_true()
    {
        Result<Test> resTest = new Test("TheName");
        resTest.HasValue.ShouldBeTrue();

        Result<string> resString = "test";
        resString.HasValue.ShouldBeTrue();

        Result<int> resInt = 1;
        resInt.HasValue.ShouldBeTrue();

        Result<decimal> resDecimal = 10.34M;
        resDecimal.HasValue.ShouldBeTrue();

        Result<bool> resBool = true;
        resBool.HasValue.ShouldBeTrue();

        Result<Unit> rUnit = Unit.Create();
        rUnit.Match(u => 1, e => -1).ShouldBe(1);

        Result<Unit> resErrUnit = new Error("error");
        resErrUnit.Match(u => 1, e => -1).ShouldBe(-1);
    }

    [Test]
    public void Should_HasValue_be_false()
    {
        Result<Test> resTest = null;
        resTest.HasValue.ShouldBeFalse();

        Result<string> resString = null;
        resString.HasValue.ShouldBeFalse();

        Result<int> resInt = 0;
        resInt.HasValue.ShouldBeFalse();

        Result<decimal> resDecimal = 0.0M;
        resDecimal.HasValue.ShouldBeFalse();

        Result<bool> resBool = false;
        resBool.HasValue.ShouldBeFalse();
    }

    [Test]
    public void Should_HasError_be_true()
    {
        Result<string> resString = new Error("Error message");
        resString.HasError.ShouldBeTrue();

        Result<string> resWithException = Result.Of("test").Then(s => ResultWithError());
        resWithException.HasError.ShouldBeTrue();
    }

    [Test]
    public void Should_HasError_be_false() 
    {
        Result<string> resString = "test";
        resString.HasError.ShouldBeFalse();

        Result<string> resWithException = Result.Of("test").Then(s => ResultWithValue());
        resWithException.HasError.ShouldBeFalse();
    }

    [Test]
    public void Should_get_non_fallback_value()
    {
        Result<string> resString = "test";
        var value = resString.GetValueOrElse("");
        value.ShouldNotBeEmpty();
        value.ShouldBe("test");
    }

    [Test]
    public void Should_get_fallback_value()
    {
        string nullValue = null;
        Result<string> resString = nullValue;
        var value = resString.GetValueOrElse("");
        value.ShouldBeEmpty();
    }

    [Test]
    public void Should_do_correct_pattern_matching_of_the_result()
    {
        string value = "test";
        string nullValue = null;

        Result<string> resValue = value;
        string strValue = resValue.Match(s => value, () => "none", e => e.Message);
        strValue.ShouldBe("test");

        Result<string> resNone = nullValue;
        string strNone = resNone.Match(s => value, () => "none", e => e.Message);
        strNone.ShouldBe("none");

        Result<string> resError = new Error("error");
        string strError = resError.Match(s => value, () => "none", e => e.Message);
        strError.ShouldBe("error");
    }

    [Test]
    public async Task Should_chaining_async_execution_with_unit()
    {
        var value = await Result.Of("test")
                              .Then(async s => await AsyncNoReturn())
                              .Match(u => "unit", () => "none", e => e.Message);

        value.ShouldBe("unit");
    }

    [Test]
    public void Should_chaining_execution_with_unit()
    {
        var value = Result.Of("test")
                      .Then(NoReturn)
                      .Match(u => "unit", () => "none", e => e.Message);

        value.ShouldBe("unit");
    }

    [Test]
    public async Task Should_chaining_async_execution_with_value()
    {
        var value = await Result.Of("test")
                            .Then(async s => await AsyncWithValue())
                            .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("async");
    }

    [Test]
    public void Should_chaining_execution_with_value()
    {
        var value = Result.Of("test")
                      .Then(s => s.ToUpper())
                      .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("TEST");
    }

    [Test]
    public async Task Should_chaining_async_execution_with_value_as_result()
    {
        var value = await Result.Of("test")
                                .Then(s => AsyncResultWithValue())
                                .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("async");
    }

    [Test]
    public void Should_chaining_execution_with_value_as_result()
    {
        var value = Result.Of("test")
                      .Then(s => ResultWithValue())
                      .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("without error");
    }

    [Test]
    public void Should_chaining_execution_with_none()
    {
        string nullValue = null;
        var value = Result.Of("test")
                      .Then(s => nullValue)
                      .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("none");
    }

    [Test]
    public void Should_chaining_execution_with_none_as_result()
    {
        var value = Result.Of("test")
                      .Then(s => ResultWithNone())
                      .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("none");
    }

    [Test]
    public void Should_chaining_execution_with_error()
    {
        var value = Result.Of("test")
                      .Then(s => WithError())
                      .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("error");
    }

    [Test]
    public void Should_chaining_execution_with_error_as_result()
    {
        var value = Result.Of("test")
                      .Then(s => ResultWithError())
                      .Match(s => s, () => "none", e => e.Message);

        value.ShouldBe("error");
    }

    [Test]
    public void Should_execute_function_OnError()
    {
        Result<string> stringResult = new Error("error");
        stringResult.OnError(err => err.Message.ShouldBe("error"));
    }

    [Test]
    public void Should_execute_function_OnSomeValue()
    {
        Result<string> stringResult = "test";
        stringResult.OnSomeValue(s => s.ShouldBe("test"));
    }

    [Test]
    public void Should_execute_function_OnNoneValue()
    {
        Result<string> stringResult = default;
        stringResult.OnNoneValue(() => Assert.Pass());
    }

    private async Task AsyncNoReturn()
    {
        await Task.CompletedTask;
    }

    private void NoReturn(string s)
    {

    }

    private async Task<string> AsyncWithValue()
    {
        return await Task.FromResult("async");
    }

    private Result<string> ResultWithError()
    {
        throw new DivideByZeroException("error");
    }

    private string WithError()
    {
        throw new DivideByZeroException("error");
    }

    private async Task<Result<string>> AsyncResultWithValue()
    {
        return await Task.FromResult("async");
    }

    private Result<string> ResultWithValue()
    {
        return "without error";
    }

    ////private async Task<Result<string>> AsyncResultWithNone()
    ////{
    ////    string nullValue = null;
    ////    return await Task.FromResult(nullValue);
    ////}

    private Result<string> ResultWithNone()
    {
        return null;
    }
}

public class Test
{
    public string Name { get; set; }

    public Test()
    { }

    public Test(string name)
    { 
        Name = name;
    }
}