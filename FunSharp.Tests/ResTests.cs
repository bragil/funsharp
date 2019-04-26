using NUnit.Framework;
using Shouldly;

namespace FunSharp.Tests
{
    public class ResTests
    {
        [Test]
        public void Deve_Ser_Resultado_Sucesso_Some()
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
        public void Deve_Ser_Resultado_Erro()
        {
            var res1 = new Res<int>(new Error(message: "Nada bom"));

            res1.IsSuccess.ShouldBe(false);
            res1.IsSome.ShouldBe(false);
            res1.IsError.ShouldBe(true);
            res1.IsNone.ShouldBe(false);

            var res2 = new Res<Unit>(new Error(message: "Nada bom de novo"));

            res2.IsSuccess.ShouldBe(false);
            res2.IsSome.ShouldBe(false);
            res2.IsError.ShouldBe(true);
            res2.IsNone.ShouldBe(false);
        }

        [Test]
        public void Deve_Ser_Resultado_None()
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
        public void Deve_Ter_Novo_Valor_Apos_Then()
        {
            var res1 = new Res<int>(256);

            var novoRes = res1.Then(n => n.ToString());

            var valor = novoRes.Match(
                                    s => s,
                                    e => e.Message,
                                    () => ""
                                );

            novoRes.IsSuccess.ShouldBeTrue();
            novoRes.IsSome.ShouldBeTrue();
            novoRes.IsError.ShouldBeFalse();
            novoRes.IsNone.ShouldBeFalse();

            valor.ShouldBe("256");
        }


        
    }
}