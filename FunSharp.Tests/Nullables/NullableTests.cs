using FunSharp.Nullables;
using FunSharp.Tests.Utils;
using NUnit.Framework;
using System;

namespace FunSharp.Tests.Nullables
{
    [TestFixture]
    public class NullableTests
    {
        [Test]
        public void Deve_Converter_Nullable_Com_Valor_Para_Opt()
        {
            int? num = 95;
            var opt = num.ToOptional();

            Assert.AreEqual(false, opt.IsNone);
            Assert.AreEqual(opt.IsSome, num.HasValue);
            Assert.AreEqual(opt.GetValueOrElse(0), num.Value);
        }

        [Test]
        public void Deve_Converter_Nullable_Sem_Valor_Para_Opt()
        {
            int? num = null;
            var opt = num.ToOptional();

            Assert.AreEqual(false, opt.IsSome);
            Assert.AreEqual(opt.IsSome, num.HasValue);
        }

        [Test]
        public void Deve_Obter_Valor_Apos_Then()
        {
            decimal? num = 456787.39M;

            var value = num.Then(d => Convert.ToInt32(d)).Value;

            Assert.AreEqual(456787, value);
        }

        [Test]
        public void Deve_Retornar_Valor_com_Match()
        {
            TesteStruct? teste = new TesteStruct() { Numero = 25, Texto = "Vinte e cinco" };

            var retorno = teste.Match(
                            t => $"{t.Numero}:{t.Texto}",
                            _ => "none"
                          );

            Assert.AreEqual("25:Vinte e cinco", retorno);
        }

        [Test]
        public void Deve_Retornar_None_com_Match()
        {
            TesteStruct? teste = default;

            var retorno = teste.Match(
                            t => $"{t.Numero}:{t.Texto}",
                            _ => "none"
                          );

            Assert.AreEqual("none", retorno);
        }

    }
}
