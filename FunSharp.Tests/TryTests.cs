using NUnit.Framework;
using Shouldly;
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
                int num1 = 10;
                int num2 = 0;
                double resultado = num1 / num2;
            }, msgError);

            res1.IsSome.ShouldBeFalse();
            res1.IsNone.ShouldBeFalse();
            res1.IsError.ShouldBeTrue();
            string msg = res1.Match(u => "Ok", err => err.Message);
            msg.ShouldBe(msgError);
        }
    }
}
