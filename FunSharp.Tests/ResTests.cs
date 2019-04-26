using NUnit.Framework;
using Shouldly;

namespace FunSharp.Tests
{
    public class ResTests
    {
        [Test]
        public void Should_Be_Some_Success_Result()
        {
            var res1 = new Res<int>(100);

            res1.IsSuccess.ShouldBe(true);
            res1.IsSome.ShouldBe(true);
            res1.IsError.ShouldBe(false);
            res1.IsNone.ShouldBe(false);

            var res2 = new Res<Unit>(Unit.Instance);

            res2.IsSuccess.ShouldBe(true);
            res2.IsSome.ShouldBe(true);
            res2.IsError.ShouldBe(false);
            res2.IsNone.ShouldBe(false);
        }

        [Test]
        public void Should_Be_Error_Result()
        {
            var res1 = new Res<int>(new Error(message: "Oh no!"));

            res1.IsSuccess.ShouldBe(false);
            res1.IsSome.ShouldBe(false);
            res1.IsError.ShouldBe(true);
            res1.IsNone.ShouldBe(false);

            var res2 = new Res<Unit>(new Error(message: "Oh no again!"));

            res2.IsSuccess.ShouldBe(false);
            res2.IsSome.ShouldBe(false);
            res2.IsError.ShouldBe(true);
            res2.IsNone.ShouldBe(false);
        }

        [Test]
        public void Should_Be_None_Result()
        {
            var res1 = new Res<int>(None.Instance);

            res1.IsSuccess.ShouldBe(true);
            res1.IsSome.ShouldBe(false);
            res1.IsError.ShouldBe(false);
            res1.IsNone.ShouldBe(true);

            var res2 = new Res<Unit>(None.Instance);

            res2.IsSuccess.ShouldBe(true);
            res2.IsSome.ShouldBe(false);
            res2.IsError.ShouldBe(false);
            res2.IsNone.ShouldBe(true);
        }

        [Test]
        public void Should_Have_New_Value_After_Then()
        {
            var res1 = new Res<int>(256);

            var newRes = res1.Then(n => n.ToString());

            var valor = newRes.Match(
                                    s => s,
                                    e => e.Message,
                                    () => ""
                                );

            newRes.IsSuccess.ShouldBeTrue();
            newRes.IsSome.ShouldBeTrue();
            newRes.IsError.ShouldBeFalse();
            newRes.IsNone.ShouldBeFalse();

            valor.ShouldBe("256");
        }


        
    }
}