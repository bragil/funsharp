using System;

namespace FunSharp
{
    /// <summary>
    /// Interface para o objeto de erro.
    /// </summary>
    public interface IError
    {
        Exception Exception { get; set; }
    }
}
