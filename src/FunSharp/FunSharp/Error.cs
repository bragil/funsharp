using System;

namespace FunSharp;

/// <summary>
/// Error data.
/// </summary>
public readonly struct Error
{
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Exception raised (optional)
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Additional error data (optional)
    /// </summary>
    public object ErrorData { get; }

    public Error(string message, Exception ex = null, object errorData = null)
    {
        Message = message;
        Exception = ex;
        ErrorData = errorData;
    }
}