using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FunSharp;
using static FunSharp.TryFunctions;
using FunSharp.Tests.Utils;

namespace FunSharp.Tests
{
    public class OptTests
    {

        [Test, Description("Deve ter valor e HasValue deve ser true")]
        public void Deve_Ter_Valor()
        {
            Opt<string> optString = "Teste";
            Assert.IsTrue(optString.IsSome);

            Opt<int> optInt = 20;
            Assert.IsTrue(optInt.IsSome);

            Opt<decimal> optDec = 1908736.97M;
            Assert.IsTrue(optDec.IsSome);

            Opt<TesteStruct> optTesteStruct = new TesteStruct() { Numero = 10, Texto = "bla bla bla" };
            Assert.IsTrue(optTesteStruct.IsSome);

            Opt<TesteEnum> optTesteEnum = TesteEnum.B;
            Assert.IsTrue(optTesteEnum.IsSome);

            Opt<TesteClass> optTeste = new TesteClass() { Id = 10 };
            Assert.IsTrue(optTeste.IsSome);
        }

        [Test, Description("Deve ter valor default e HasValue deve ser false")]
        public void Deve_Ser_None()
        {
            string str = null;
            Opt<string> optString = str;
            Assert.IsFalse(optString.IsSome);

            Opt<int> optInt = 0;
            Assert.IsFalse(optInt.IsSome);

            Opt<decimal> optDec = 0.0M;
            Assert.IsFalse(optDec.IsSome);

            TesteStruct testeStruct = default;
            Opt<TesteStruct> optTesteStruct = testeStruct;
            Assert.IsFalse(optTesteStruct.IsSome);

            TesteEnum testeEnum = default;
            Opt<TesteEnum> optTesteEnum = testeEnum;
            Assert.IsFalse(optTesteEnum.IsSome);

            TesteClass testeClass = default;
            Opt<TesteClass> optTesteClass = testeClass;
            Assert.IsFalse(optTesteClass.IsSome);
        }

        [Test, Description("Deve executar OnValue quando há valor")]
        public void Deve_Executar_OnValue()
        {
            Opt<string> opt = "teste";

            opt.OnSome(str =>
            {
                Assert.Pass("Ok");
            })
            .OnNone(() =>
            {
                Assert.Fail("Erro");
            });
        }

        [Test, Description("Deve executar OnNone quando não há valor (None)")]
        public void Deve_Executar_OnNone()
        {
            string teste = null;
            Opt<string> opt = teste;

            opt.OnSome(str =>
            {
                Assert.Fail("Erro");
            })
            .OnNone(() =>
            {
                Assert.Pass("Ok");
            });
        }

        [Test, Description("Deve retornar o valor via GetValueOrElse")]
        public void Deve_Retornar_Valor_Via_GetValueOrElse()
        {
            TesteClass teste = new TesteClass() { Id = 20 };
            Opt<TesteClass> opt = teste;

            var retorno = opt.GetValueOrElse(default);
            Assert.IsNotNull(retorno);
            Assert.AreEqual(20, retorno.Id);
        }

        [Test, Description("Deve retornar o fallback via GetValueOrElse")]
        public void Deve_Retornar_Fallback_Via_GetValueOrElse()
        {
            TesteClass teste = null;
            Opt<TesteClass> opt = teste;

            var retorno = opt.GetValueOrElse(default);
            Assert.IsNull(retorno);
        }

        [Test, Description("Deve retornar o valor de some via Match")]
        public void Deve_Retornar_Valor_De_Some_Via_Match()
        {
            TesteClass teste = new TesteClass() { Id = 20 };
            Opt<TesteClass> opt = teste;
            var ret = opt.Match(
                        t => "some",
                        _ => "none"
                      );

            Assert.AreEqual("some", ret);
        }

        [Test, Description("Deve retornar o valor de none via Match")]
        public void Deve_Retornar_Valor_De_None_Via_Match()
        {
            TesteClass teste = null;
            Opt<TesteClass> opt = teste;
            var ret = opt.Match(
                        t => "some",
                        _ => "none"
                      );

            Assert.AreEqual("none", ret);
        }

        [Test, Description("Deve retornar o valor correto via Then")]
        public void Deve_Retornar_Valor_Correto_Via_Then()
        {
            TesteClass teste = new TesteClass() { Id = 20 };
            Opt<TesteClass> opt = teste;
            var newOpt = opt.Then(t => "Id = 20");
            var retorno1 = newOpt.GetValueOrElse("");
            Assert.AreEqual("Id = 20", retorno1);

            Opt<int> optInt = 1250;
            Opt<int> optInt2 = 5000;
            var newOpt2 = optInt.Then(n => optInt2);
            var retorno2 = newOpt2.GetValueOrElse(0);
            Assert.AreEqual(5000, retorno2);
        }

        [Test, Description("Deve retornar o Opt do valor via Opt.Of")]
        public void Deve_Retornar_Opt_Do_Valor_Via_Of()
        {
            var opt = Opt.Of(295.39M);
            Assert.IsTrue(opt.IsSome);
            Assert.AreEqual(295.39M, opt.GetValueOrElse(0M));
        }

        [Test, Description("Deve retornoar o Opt com None via Opt.Empty")]
        public void Deve_Retornar_Opt_Com_None_Via_Empty()
        {
            var opt = Opt.Empty<string>();
            Assert.IsFalse(opt.IsSome);
        }
    }

    enum Enumeracao
    {
        ItemUm, ItemDois, ItemTres
    }

    struct Point
    { }

    class Tipo
    { 
        public int Numero { get; set; }
    }
}
