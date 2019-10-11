using NUnit.Framework;
using Shouldly;
using System;

namespace FunSharp.Tests
{
    public class ErrorTests
    {
        [Test]
        public void Mensagem_Erro_Deve_Ser_Mensagem_Da_Exception()
        {
            var message = "bla bla bla";
            var error = new Error(new Exception(message));

            error.Message.ShouldBe(message);
            error.Exception.ShouldNotBeNull();
            error.ErrorData.ShouldBeNull();
        }

        [Test]
        public void Exception_ErrorData_Devem_Ser_Nulos()
        {
            var message = "bla bla bla";
            var error = new Error(message: message);

            error.Exception.ShouldBeNull();
            error.ErrorData.ShouldBeNull();
            error.Message.ShouldBe(message);
        }

        [Test]
        public void Error_Deve_Ter_Todas_As_Propriedades_Nulas()
        {
            var error = new Error();
            error.Exception.ShouldBeNull();
            error.ErrorData.ShouldBeNull();
            error.Message.ShouldBeNull();
        }
    }
}
