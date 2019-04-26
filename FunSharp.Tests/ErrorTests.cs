using FunSharp;
using NUnit.Framework;
using Shouldly;
using System;

namespace FunSharp.Tests
{
    public class ErrorTests
    {
        [Test]
        public void Deve_Ter_Mensagem_da_Exception()
        {
            var mensagem = "bla bla bla";
            var erro = new Error(new Exception(mensagem));

            erro.Message.ShouldBe(mensagem);
        }

        [Test]
        public void Deve_Ter_Exception_ErrorData_Nulos()
        {
            var mensagem = "bla bla bla";
            var erro = new Error(message: mensagem);

            erro.Exception.ShouldBeNull();
            erro.ErrorData.ShouldBeNull();
        }
    }
}
