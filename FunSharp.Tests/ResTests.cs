using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FunSharp;
using static FunSharp.TryFunctions;

namespace FunSharp.Tests
{
    public class ResTests
    {
        [Test]
        public void Deve_Ser_Some()
        {
            var res1 = Res.Of(100);
            res1.IsSome.ShouldBeTrue();
            res1.IsError.ShouldBeFalse();
            res1.IsNone.ShouldBeFalse();

            var res2 = Res.Of(Unit.Instance);
            res2.IsSome.ShouldBeTrue();
            res2.IsError.ShouldBeFalse();
            res2.IsNone.ShouldBeFalse();

            var res3 = Res.Of(new Teste());
            res3.IsSome.ShouldBeTrue();
            res3.IsError.ShouldBeFalse();
            res3.IsNone.ShouldBeFalse();
        }

        [Test]
        public void Deve_Ser_Error()
        {
            Res<string> res1 = new Error(message: "Oh no!");
            res1.IsError.ShouldBeTrue();
            res1.IsSome.ShouldBeFalse();
            res1.IsNone.ShouldBeFalse();

            Res<Teste> res2 = new Error(message: "Oh no again!");
            res2.IsError.ShouldBeTrue();
            res2.IsSome.ShouldBeFalse();
            res2.IsNone.ShouldBeFalse();

            Res<int> res3 = new Exception("Oh no again again!");
            res3.IsError.ShouldBeTrue();
            res3.IsSome.ShouldBeFalse();
            res3.IsNone.ShouldBeFalse();
        }

        [Test]
        public void Deve_Ser_None()
        {
            var res1 = Res.Of(None.Instance);
            res1.IsSome.ShouldBeFalse();
            res1.IsError.ShouldBeFalse();
            res1.IsNone.ShouldBeTrue();

            var res2 = Res.Of(None.Instance);
            res2.IsSome.ShouldBeFalse();
            res2.IsError.ShouldBeFalse();
            res2.IsNone.ShouldBeTrue();

            Teste t = null;
            var res3 = Res.Of(t);
            res3.IsSome.ShouldBeFalse();
            res3.IsError.ShouldBeFalse();
            res3.IsNone.ShouldBeTrue();
        }

    }

    class Teste: IDisposable
    {
        public int Soma(int x, int y)
            => x + y;

        public void Dispose()
        { }
    }
}