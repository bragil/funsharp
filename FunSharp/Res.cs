using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static FunSharp.TryFunctions;

namespace FunSharp
{
    /// <summary>
    /// Resultado de uma computação.
    /// </summary>
    /// <typeparam name="TValue">Tipo do objeto de valor</typeparam>
    public struct Res<TValue>
    {
        private Error Error { get; set; }
        private TValue Value { get; set; }
        public bool IsSuccess { get; }
        public bool IsError => !IsSuccess;
        public bool IsSome { get; }
        public bool IsNone { get; }

        public Res(TValue value)
        {
            Value = value;
            Error = default;
            IsSuccess = true;
            IsNone = Value == null || (Value.Equals(default(TValue)) && Value.GetType() != typeof(Unit));
            IsSome = !IsNone;
        }

        public Res(Error error)
        {
            Value = default;
            Error = error;
            IsSuccess = false;
            IsSome = false;
            IsNone = false;
        }

        public Res(None none)
        {
            Value = default;
            Error = default;
            IsSuccess = true;
            IsSome = false;
            IsNone = true;
        }

        /// <summary>
        /// Tenta obter o valor. Em caso de erro, retorna o fallback informado.
        /// </summary>
        /// <param name="fallback">Valor a ser retornado em caso de erro.</param>
        /// <returns>Valor</returns>
        public TValue GetValueOrElse(TValue fallback)
            => IsError ? fallback : Value;

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="withValue">Função a ser executada em execuções bem-sucedidas com valor de retorno.</param>
        /// <param name="withError">Função a ser executada em execuções com erro.</param>
        /// <param name="noValue">Função a ser executada em execuções bem-sucedidas que não retornam valor.</param>
        /// <returns>T</returns>
        public T Match<T>(Func<TValue, T> withValue, Func<Error, T> withError, Func<T> noValue = null)
        {
            if (IsError)
                return withError(Error);
            else if (IsSome)
                return withValue(Value);
            else if (noValue != null)
                return noValue();
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Res[T]</returns>
        public Res<T> Then<T>(Func<TValue, T> function)
        {
            var val = Value;
            if (IsSuccess)
            {
                if (IsSome)
                    return Try(() => function(val));
                else
                    return None.Instance;
            }
            return Error;
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Res[T]</returns>
        public Res<T> Then<T>(Func<TValue, Res<T>> function)
        {
            var val = Value;
            if (IsSuccess)
            {
                if (IsSome)
                    return function(val);
                else
                    return None.Instance;
            }
            return Error;
        }

        /// <summary>
        /// Executa a action e retorna Unit.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="action">Action a ser executada</param>
        /// <returns>Res[Unit]</returns>
        public Res<Unit> Then(Action<TValue> action)
            => Then(Utils.ToFunc(action));

        /// <summary>
        /// Intercepta um fluxo com erro.
        /// </summary>
        /// <param name="function">Função a ser executada no caso de erro.</param>
        /// <returns>Res[TValue]</returns>
        public Res<TValue> Fail(Action<Error> function)
        {

            if (IsError)
            {
                var e = Error;
                return Try(() => { function(e); return e; });
            }
            return Value;
        }

        /// <summary>
        /// Operador de cast implícito para sucesso.
        /// </summary>
        /// <param name="success">Valor de retorno para sucesso</param>
        public static implicit operator Res<TValue>(TValue success) 
            => new Res<TValue>(success);

        /// <summary>
        /// Operador de cast implícito para erro.
        /// </summary>
        /// <param name="error">Objeto de erro</param>
        public static implicit operator Res<TValue>(Error error) 
            => new Res<TValue>(error);

        /// <summary>
        /// Operador de cast implícito para erro.
        /// </summary>
        /// <param name="exception">Objeto de Exception</param>
        public static implicit operator Res<TValue>(Exception exception)
            => new Res<TValue>(new Error(exception, exception.Message));

        /// <summary>
        /// Operador de cast implícito para None (nenhum valor).
        /// </summary>
        /// <param name="none">Objeto None</param>
        public static implicit operator Res<TValue>(None none) 
            => new Res<TValue>(None.Instance);
    }

    public struct Res
    {
        /// <summary>
        /// Cria um Res[T] com o valor passado.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="value">Objeto do valor</param>
        /// <returns>Res[T]</returns>
        public static Res<T> Of<T>(T value)
            => new Res<T>(value);
    }
}
