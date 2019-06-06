using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static FunSharp.TryFunctions;

namespace FunSharp.Tests
{
    public class ResTests
    {
        [Test]
        public void Deve_Ser_Some()
        {
            var res1 = new Res<int>(100);

            res1.IsSome.ShouldBeTrue();
            res1.IsError.ShouldBeFalse();
            res1.IsNone.ShouldBeFalse();

            var res2 = new Res<Unit>(Unit.Instance);

            res2.IsSome.ShouldBeTrue();
            res2.IsError.ShouldBeFalse();
            res2.IsNone.ShouldBeFalse();
        }

        [Test]
        public void Deve_Ser_Error()
        {
            var res1 = new Res<int>(new Error(message: "Oh no!"));

            res1.IsError.ShouldBeTrue();
            res1.IsSome.ShouldBeFalse();
            res1.IsNone.ShouldBeFalse();

            var res2 = new Res<Unit>(new Error(message: "Oh no again!"));

            res2.IsError.ShouldBeTrue();
            res2.IsSome.ShouldBeFalse();
            res2.IsNone.ShouldBeFalse();
        }

        [Test]
        public void Deve_Ser_None()
        {
            var res1 = new Res<int>(None.Instance);

            res1.IsSome.ShouldBeFalse();
            res1.IsError.ShouldBeFalse();
            res1.IsNone.ShouldBeTrue();

            var res2 = new Res<Unit>(None.Instance);

            res2.IsSome.ShouldBeFalse();
            res2.IsError.ShouldBeFalse();
            res2.IsNone.ShouldBeTrue();
        }

        [Test]
        public void Deve_Ter_Novo_Valor_Apos_Then()
        {
            var res1 = new Res<int>(256);

            var newRes = res1.Then(n => n.ToString());

            var valor = newRes.Match(
                                    s => s,
                                    e => e.Message,
                                    _ => ""
                                );

            newRes.IsSome.ShouldBeTrue();
            newRes.IsError.ShouldBeFalse();
            newRes.IsNone.ShouldBeFalse();

            valor.ShouldBe("256");
        }


        [Test]
        public void Deve_Ser_Unit()
        {
            var res = Res.Of(500)
                         .Then(n => Debug.WriteLine("text"));

            res.IsSome.ShouldBeTrue();
            res.IsError.ShouldBeFalse();
            res.IsNone.ShouldBeFalse();

            var value = res.Match(
                            some:  v => v.GetType().Name,
                            error: e => e.GetType().Name,
                            none:  _ => "None"
                        );

            value.ShouldBe("Unit");
        }

        [Test]
        public void Deve_Ser_String_Apos_Unit()
        {
            Use(new Teste(), disp => disp.Soma(1, 2));


            var res = Res.Of(500)
                         .Then(n => Debug.WriteLine("text"))
                         .Then(u => u.GetType().Name);

            res.IsSome.ShouldBeTrue();
            res.IsError.ShouldBeFalse();
            res.IsNone.ShouldBeFalse();

            var value = res.GetValueOrElse("");
            value.ShouldBe("Unit");
        }

        [Test]
        public void Deve_Retornar_Resultado_Task()
        {
            var task = Task.Run<int>(() => { Task.Delay(20000); return 10 * 123; });
            int number = task.FromTask().GetValueOrElse(-1);

            Assert.AreEqual(1230, number);
        }

        [Test]
        public void Deve_Retornar_Error_Task()
        {
            var task = Task.Run<int>(() => { throw new InvalidOperationException("invalid"); return 10 * 123; });
            int number = task.FromTask().GetValueOrElse(-1);

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task Deve_Retornar_Task_Apos_Res()
        {
            var res = await Res.Of("bla")
                               .ThenAsync(s => s.Length);

            int length = res.GetValueOrElse(-1);
            Assert.AreEqual(3, length);
        }

        [Test]
        public async Task Deve_Retornar_Res_De_Task_Res()
        {
            var taskRes = Task.FromResult(Res.Of(10));

            var number = (await taskRes.ThenAsync(n => n * Math.PI)
                                       .ThenAsync(d => Convert.ToInt32(d)))
                                       .GetValueOrElse(-1);

            Assert.AreEqual(31, number);
        }

        [Test]
        public async Task Deve_Retornar_Unit_De_Task_Res()
        {
            var taskRes = Task.FromResult(Res.Of(10));           

            var number = (await taskRes.ThenAsync(n => n * Math.PI)
                                       .ThenAsync(d => Debug.Write(d)))
                                       .Match(
                                            some: v => 1,
                                            error: e => -1,
                                            none: _ => 0
                                       );

            Assert.AreEqual(1, number);
        }

        [Test]
        public async Task Deve_Transformar_Task_T_Em_Task_Res_T()
        {
            var task = Task.FromResult(5);
            var number = (await Res.OfAsync(task)).GetValueOrElse(-1);

            Assert.AreEqual(5, number);
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