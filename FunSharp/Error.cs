using System;
using System.Collections.Generic;
using System.Text;

namespace FunSharp
{
    public class Error 
    {
        public Exception Exception { get; set; }

        public string Message { get; set; }

        public object ErrorData { get; set; }

        public Error(Exception exception = null, string message = null, object errorData = null)
        {
            Exception = exception;
            Message = message ?? exception?.Message;
            ErrorData = errorData;
        }
    }
}
