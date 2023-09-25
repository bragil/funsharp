using FunSharp;
using Shouldly;

namespace Tests;

public class ChooseTests
{

    [Test]
    public void Should_return_correct_value_with_2_types()
    {
        Choose<int, string> choose = 50;
        var value = choose.Match(i => "int", s => "string");
        value.ShouldBe("int");

        Choose<int, string> choose2 = "string";
        var value2 = choose2.Match(i => "int", s => "string");
        value2.ShouldBe("string");
    }

    [Test]
    public void Should_return_correct_value_with_3_types()
    {
        Choose<int, string, DateTime> choose = DateTime.Now;
        var value = choose.Match(i => "int", s => "string", d => "datetime");
        value.ShouldBe("datetime");

        Choose<int, string, DateTime> choose2 = 100;
        var value2 = choose2.Match(i => "int", s => "string", d => "datetime");
        value2.ShouldBe("int");

        Choose<int, string, DateTime> choose3 = "string";
        var value3 = choose3.Match(i => "int", s => "string", d => "datetime");
        value3.ShouldBe("string");
    }

    [Test]
    public void Should_return_correct_value_with_4_types()
    {
        Choose<int, string, DateTime, Test> choose = DateTime.Now;
        var value = choose.Match(i => "int", s => "string", d => "datetime", t => "test");
        value.ShouldBe("datetime");

        Choose<int, string, DateTime, Test> choose2 = 100;
        var value2 = choose2.Match(i => "int", s => "string", d => "datetime", t => "test");
        value2.ShouldBe("int");

        Choose<int, string, DateTime, Test> choose3 = "string";
        var value3 = choose3.Match(i => "int", s => "string", d => "datetime", t => "test");
        value3.ShouldBe("string");

        Choose<int, string, DateTime, Test> choose4 = new Test("Name");
        var value4 = choose4.Match(i => "int", s => "string", d => "datetime", t => "test");
        value4.ShouldBe("test");
    }

    [Test]
    public void Should_return_correct_value_with_5_types()
    {
        Choose<int, string, DateTime, Test, char> choose = DateTime.Now;
        var value = choose.Match(i => "int", s => "string", d => "datetime", t => "test", c => "char");
        value.ShouldBe("datetime");

        Choose<int, string, DateTime, Test, char> choose2 = 100;
        var value2 = choose2.Match(i => "int", s => "string", d => "datetime", t => "test", c => "char");
        value2.ShouldBe("int");

        Choose<int, string, DateTime, Test, char> choose3 = "string";
        var value3 = choose3.Match(i => "int", s => "string", d => "datetime", t => "test", c => "char");
        value3.ShouldBe("string");

        Choose<int, string, DateTime, Test, char> choose4 = new Test("Name");
        var value4 = choose4.Match(i => "int", s => "string", d => "datetime", t => "test", c => "char");
        value4.ShouldBe("test");

        Choose<int, string, DateTime, Test, char> choose5 = 'A';
        var value5 = choose5.Match(i => "int", s => "string", d => "datetime", t => "test", c => "char");
        value5.ShouldBe("char");
    }
}
