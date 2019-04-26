using NUnit.Framework;
using Shouldly;
using System;

namespace FunSharp.Tests
{
    public class ErrorTests
    {
        [Test]
        public void Should_Have_Exception_Message()
        {
            var message = "bla bla bla";
            var error = new Error(new Exception(message));

            error.Message.ShouldBe(message);
        }

        [Test]
        public void Should_Have_Null_ErrorData()
        {
            var message = "bla bla bla";
            var error = new Error(message: message);

            error.Exception.ShouldBeNull();
            error.ErrorData.ShouldBeNull();
        }
    }
}
