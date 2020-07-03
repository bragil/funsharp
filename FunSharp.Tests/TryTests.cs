using NUnit.Framework;
using Shouldly;
using System;
using static FunSharp.TryFunctions;

namespace FunSharp.Tests
{
    public class TryTests
    {
        [Test]
        public void Deve_Retornar_Error_Preenchido_Corretamente()
        {
            string msgError = "Erro ao executar operação.";
            var res1 = Try(() =>
            {
                throw new DivideByZeroException("Não pode dividir por zero.");
            }, errFunction: null, msgError);

            res1.IsSome.ShouldBeFalse();
            res1.IsNone.ShouldBeFalse();
            res1.IsError.ShouldBeTrue();
            string msg = res1.Match(u => "Ok", err => err.Message);
            msg.ShouldBe(msgError);
        }
    }
}
