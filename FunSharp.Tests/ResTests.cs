using FunSharp.Tests.Utils;
using NUnit.Framework;
using System;

namespace FunSharp.Tests
{
    [TestFixture]
    public class ResTests
    {
        [Test, Description("Deve existir valor no resultado")]
        public void Deve_Existir_Valor_No_Resultado()
        {
            Res<string> res = "Teste";
            Assert.IsTrue(res.IsSome);
            Assert.IsFalse(res.IsError);
            Assert.AreEqual("Teste", res.GetValueOrElse(""));

            Res<int> res2 = 233;
            Assert.IsTrue(res2.IsSome);
            Assert.IsFalse(res2.IsError);
            Assert.AreEqual(233, res2.GetValueOrElse(0));

            Res<TesteClass> res3 = new TesteClass() { Id = 100 };
            Assert.IsTrue(res3.IsSome);
            Assert.IsFalse(res3.IsError);
            var testeClass = res3.GetValueOrElse(default);
            Assert.IsNotNull(testeClass);
            Assert.AreEqual(100, testeClass.Id);

            Res<TesteStruct> res4 = new TesteStruct() { Numero = 3, Texto = "Texto" };
            Assert.IsTrue(res4.IsSome);
            Assert.IsFalse(res4.IsError);
            var testeStruct = res4.GetValueOrElse(default);
            Assert.AreEqual(3, testeStruct.Numero);
            Assert.AreEqual("Texto", testeStruct.Texto);

            Res<TesteEnum> res5 = TesteEnum.B;
            Assert.IsTrue(res5.IsSome);
            Assert.IsFalse(res5.IsError);
            Assert.AreEqual(TesteEnum.B, res5.GetValueOrElse(TesteEnum.A));

            Res<DateTime> res6 = new DateTime(2020, 6, 19);
            Assert.IsTrue(res6.IsSome);
            Assert.IsFalse(res6.IsError);
            Assert.AreEqual(2020, res6.GetValueOrElse(DateTime.MinValue).Year);
        }

        [Test, Description("Deve estar sem valor o resultado")]
        public void Deve_Estar_Sem_Valor_Resultado()
        {
            Res<string> res = default(string);
            Assert.IsFalse(res.IsSome);
            Assert.IsFalse(res.IsError);

            Res<int> res2 = 0;
            Assert.IsFalse(res2.IsSome);
            Assert.IsFalse(res2.IsError);

            Res<TesteClass> res3 = default(TesteClass);
            Assert.IsFalse(res3.IsSome);
            Assert.IsFalse(res3.IsError);

            Res<TesteStruct> res4 = default(TesteStruct);
            Assert.IsFalse(res4.IsSome);
            Assert.IsFalse(res4.IsError);

            Res<TesteEnum> res5 = default(TesteEnum);
            Assert.IsFalse(res5.IsSome);
            Assert.IsFalse(res5.IsError);

            Res<DateTime> res6 = DateTime.MinValue;
            Assert.IsFalse(res6.IsSome);
            Assert.IsFalse(res6.IsError);
        }

        [Test, Description("Deve existir erro no resultado")]
        public void Deve_Existir_Erro_No_Resultado()
        {
            Res<string> res = new Error(message:"mensagem");
            Assert.IsFalse(res.IsSome);
            Assert.IsTrue(res.IsError);
            res.OnError(err => Assert.Pass());

            Res<int> res2 = new Error(message:"Erro");
            Assert.IsFalse(res2.IsSome);
            Assert.IsTrue(res2.IsError);
            res2.OnError(err => Assert.Pass());

            Res<TesteStruct> res3 = new Error(message:"Dados inválidos");
            Assert.IsFalse(res3.IsSome);
            Assert.IsTrue(res3.IsError);
            res3.OnError(err => Assert.Pass());
        }


        [Test, Description("Deve retornar valor com GetValueOrElse")]
        public void Deve_Retornar_Valor_Com_GetValueOrElse()
        {
            Res<string> res = "Teste";
            Assert.AreEqual("Teste", res.GetValueOrElse(""));

            Res<int> res2 = 233;
            Assert.AreEqual(233, res2.GetValueOrElse(0));

            Res<TesteClass> res3 = new TesteClass() { Id = 100 };
            var testeClass = res3.GetValueOrElse(default);
            Assert.IsNotNull(testeClass);
            Assert.AreEqual(100, testeClass.Id);

            Res<TesteStruct> res4 = new TesteStruct() { Numero = 3, Texto = "Texto" };
            var testeStruct = res4.GetValueOrElse(default);
            Assert.AreEqual(3, testeStruct.Numero);
            Assert.AreEqual("Texto", testeStruct.Texto);

            Res<TesteEnum> res5 = TesteEnum.B;
            Assert.AreEqual(TesteEnum.B, res5.GetValueOrElse(TesteEnum.A));

            Res<DateTime> res6 = new DateTime(2020, 6, 19);
            Assert.AreEqual(2020, res6.GetValueOrElse(DateTime.MinValue).Year);
        }

        [Test, Description("Deve retornar valor com GetValueOrElse")]
        public void Deve_Retornar_Fallback_Com_GetValueOrElse()
        {
            Res<string> res = default(string);
            Assert.AreEqual("", res.GetValueOrElse(""));

            Res<int> res2 = default(int);
            Assert.AreEqual(0, res2.GetValueOrElse(0));

            Res<TesteClass> res3 = default(TesteClass);
            var testeClass = res3.GetValueOrElse(default);
            Assert.IsNull(testeClass);

            Res<TesteStruct> res4 = default(TesteStruct);
            var testeStruct = res4.GetValueOrElse(default);
            Assert.AreEqual(0, testeStruct.Numero);

            Res<TesteEnum> res5 = default(TesteEnum);
            Assert.AreEqual(TesteEnum.B, res5.GetValueOrElse(TesteEnum.B));

            Res<DateTime> res6 = DateTime.MinValue;
            var data = res6.GetValueOrElse(new DateTime(2020, 1, 1));
            Assert.AreEqual(2020, data.Year);
        }

        [Test, Description("Deve retornar valor com GetValueOrMatch")]
        public void Deve_Retornar_Valor_Com_GetValueOrMatch()
        {
            Res<string> res = "Teste";
            var retorno = res.GetValueOrMatch(err => err.Message, _ => "");
            Assert.AreEqual("Teste", retorno);

            Res<int> res2 = 250;
            var retorno2 = res2.GetValueOrMatch(err => -1, _ => 0);
            Assert.AreEqual(250, retorno2);
        }

        [Test, Description("Deve retornar None com GetValueOrMatch")]
        public void Deve_Retornar_None_Com_GetValueOrMatch()
        {
            Res<string> res = default(string);
            var retorno = res.GetValueOrMatch(err => err.Message, _ => "");
            Assert.AreEqual("", retorno);

            Res<int> res2 = 0;
            var retorno2 = res2.GetValueOrMatch(err => -1, _ => 0);
            Assert.AreEqual(0, retorno2);
        }

        [Test, Description("Deve retornar erro com GetValueOrMatch")]
        public void Deve_Retornar_Erro_Com_GetValueOrMatch()
        {
            Res<string> res = new Error(message: "Erro");
            var retorno = res.GetValueOrMatch(err => err.Message, _ => "");
            Assert.AreEqual("Erro", retorno);

            Res<int> res2 = new Error(message: "Erro");
            var retorno2 = res2.GetValueOrMatch(err => -1, _ => 0);
            Assert.AreEqual(-1, retorno2);
        }

        [Test, Description("Deve retornar valor com Match")]
        public void Deve_Retornar_Valor_Com_Match()
        {
            Res<string> res = "Teste";
            var retorno = res.Match(val => val, err => err.Message, _ => "");
            Assert.AreEqual("Teste", retorno);

            Res<int> res2 = 250;
            var retorno2 = res2.Match(val => val, err => -1, _ => 0);
            Assert.AreEqual(250, retorno2);
        }

        [Test, Description("Deve retornar erro com Match")]
        public void Deve_Retornar_Erro_Com_Match()
        {
            Res<string> res = new Error(message: "Erro");
            var retorno = res.Match(val => val, err => err.Message, _ => "");
            Assert.AreEqual("Erro", retorno);

            Res<int> res2 = new Error(message: "Erro");
            var retorno2 = res2.Match(val => val, err => -1, _ => 0);
            Assert.AreEqual(-1, retorno2);
        }

        [Test, Description("Deve retornar None com Match")]
        public void Deve_Retornar_None_Com_Match()
        {
            Res<string> res = default(string);
            var retorno = res.Match(val => val, err => err.Message, _ => "");
            Assert.AreEqual("", retorno);

            Res<int> res2 = 0;
            var retorno2 = res2.Match(val => val, err => -1, _ => 0);
            Assert.AreEqual(0, retorno2);
        }

        [Test, Description("Deve executar OnError")]
        public void Deve_Executar_OnError()
        {
            Res<string> res = new Error(message: "Erro");
            res.OnError(err => Assert.Pass()).OnSuccess(s => Assert.Fail("Não deve executar OnSuccess"));

            Res<int> res2 = new Error(message: "Erro");
            res2.OnError(err => Assert.Pass()).OnSuccess(s => Assert.Fail("Não deve executar OnSuccess"));
        }

        [Test, Description("Deve executar OnSuccess com valor")]
        public void Deve_Executar_OnSuccess_Com_Valor()
        {
            Res<string> res = "Teste";
            res.OnError(err => Assert.Fail("Não deve executar OnError"))
               .OnSuccess(s => Assert.Pass(), () => Assert.Fail("Não deve executar noneFunction"));

            Res<int> res2 = 20;
            res2.OnError(err => Assert.Fail("Não deve executar OnError"))
                .OnSuccess(s => Assert.Pass(), () => Assert.Fail("Não deve executar noneFunction"));
        }

        [Test, Description("Deve executar OnSuccess com None")]
        public void Deve_Executar_OnSuccess_Com_None()
        {
            Res<string> res = default(string);
            res.OnError(err => Assert.Fail("Não deve executar OnError"))
               .OnSuccess(s => Assert.Fail("Não deve executar someFunction"), () => Assert.Pass());

            Res<int> res2 = 0;
            res2.OnError(err => Assert.Fail("Não deve executar OnError"))
                .OnSuccess(s => Assert.Fail("Não deve executar someFunction"), () => Assert.Pass());
        }

        [Test, Description("Deve retornar valor correto de Combine")]
        public void Deve_Retornar_Valor_Correto_De_Combine()
        {
            Res<string> res = "TESTE";
            Res<DateTime> res2 = new DateTime(2020, 1, 1);
            Res<int> res3 = 15;

            var ret = res.Combine(res2).Combine(res3).GetValueOrElse(0);
            Assert.AreEqual(15, ret);
        }

        [Test, Description("Deve retornar valor de erro de Combine")]
        public void Deve_Retornar_Valor_De_Erro_De_Combine()
        {
            Res<string> res = "TESTE";
            Res<DateTime> res2 = new Error(message: "Erro");
            Res<int> res3 = 15;

            var ret = res.Combine(res2).Combine(res3).GetValueOrMatch(err => -1, _ => 0);
            Assert.AreEqual(-1, ret);
        }

        [Test, Description("Deve retornar valor de None do último Combine")]
        public void Deve_Retornar_Valor_De_None_De_Combine()
        {
            Res<string> res = "TESTE";
            Res<DateTime> res2 = new DateTime(2020, 1, 1);
            Res<int> res3 = 0;

            var ret = res.Combine(res2).Combine(res3).GetValueOrMatch(err => -1, _ => 0);
            Assert.AreEqual(0, ret);
        }

        [Test, Description("Deve retornar valor correto de Then")]
        public void Deve_Retornar_Valor_Correto_De_Then()
        {
            Res<string> res = "TESTE";

            var ret = res.Then(s => s.Length)
                         .Match(
                            val => val,
                            err => -1,
                            _ => 0
                         );
            Assert.AreEqual(5, ret);
        }

        [Test, Description("Deve retornar valor correto de Then após erro")]
        public void Deve_Retornar_Valor_Correto_De_Then_Apos_Erro()
        {
            Res<string> res = "TESTE";

            var ret = res.Then(s => s.Length)
                         .Then(l =>
                         {
                             int zero = 0;
                             int _ = 10 / zero;  // DivideByZeroException
                             return $"Tamanho: {l}";
                         })
                         .Then(t => new TesteStruct() { Numero = 1, Texto = t })
                         .Match(
                            val => val.Numero,
                            err => -1,
                            _ => 0
                         );
            Assert.AreEqual(-1, ret);
        }

        [Test, Description("Deve retornar valor correto de Then após receber outro Then")]
        public void Deve_Retornar_Valor_Correto_De_Then_Apos_Receber_Outro_Res()
        {
            Res<string> res = "TESTE";
            Res<int> res2 = 15;
            Res<DateTime> res3 = new DateTime(2020, 1, 1);
            Res<TesteClass> res4 = new TesteClass() { Id = 20 };
            Res<decimal> res5 = 1234.99M;

            var ret = res.Then(x => res2)
                         .Then(y => res3)
                         .Then(z => res4)
                         .Then(w => res5)
                         .GetValueOrElse(0M);

            Assert.AreEqual(1234.99M, ret);
        }
    }
}