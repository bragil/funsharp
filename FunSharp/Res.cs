using System;
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
        private Opt<TValue> Value { get; set; }

        public bool IsError { get; }
        public bool IsSome { get; }
        public bool IsNone { get; }

        public Res(TValue value)
        {
            Value = Option.Of(value);
            Error = default;
            IsError = false;
            IsNone = Value.IsNone;
            IsSome = Value.IsSome;
        }

        public Res(Error error)
        {
            Value = Option.Empty<TValue>();
            Error = error;
            IsError = true;
            IsNone = false;
            IsSome = false;
        }

        public Res(None none)
        {
            Value = Option.Empty<TValue>();
            Error = default;
            IsError = false;
            IsNone = Value.IsNone;
            IsSome = Value.IsSome;
        }

        /// <summary>
        /// Tenta obter o valor. Em caso de erro, retorna o fallback informado.
        /// </summary>
        /// <param name="fallback">Valor a ser retornado em caso de erro.</param>
        /// <returns>Valor</returns>
        public TValue GetValueOrElse(TValue fallback)
            => IsError ? fallback : Value.GetValueOrElse(fallback);

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="some">Função a ser executada em execuções bem-sucedidas com valor de retorno.</param>
        /// <param name="error">Função a ser executada em execuções com erro.</param>
        /// <param name="none">Função a ser executada em execuções bem-sucedidas que não retornam valor.</param>
        /// <returns>T</returns>
        public T Match<T>(Func<TValue, T> some, Func<Error, T> error, Func<None, T> none = null)
            => IsError ? error(Error) : Value.Match(some, none);

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Res[T]</returns>
        public Res<T> Then<T>(Func<TValue, T> function)
        {
            if (IsError) return Error;

            var val = Value.GetValueOrElse(default);
            return IsSome ? Try(() => function(val)) : None.Instance;
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
            if (IsError)
                return Error;
            else
                return IsSome ? function(Value.GetValueOrElse(default)) : None.Instance;
        }

        /// <summary>
        /// Executa a action e retorna Unit.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="action">Action a ser executada</param>
        /// <returns>Res[Unit]</returns>
        public Res<Unit> Then(Action<TValue> action)
            => Then(FunSharpUtils.ToFunc(action));

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Condição</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[T]</returns>
        public Res<T> IfThen<T>(bool condition, Func<TValue, T> isTrue, Func<TValue, T> isFalse)
            => condition ? Then(isTrue) : Then(isFalse);

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Condição</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[T]</returns>
        public Res<T> IfThen<T>(bool condition, Func<TValue, Res<T>> isTrue, Func<TValue, Res<T>> isFalse)
            => condition ? Then(isTrue) : Then(isFalse);

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Condição</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[Unit]</returns>
        public Res<Unit> IfThen(bool condition, Action<TValue> isTrue, Action<TValue> isFalse)
            => condition ? Then(isTrue) : Then(isFalse);

        /// <summary>
        /// Intercepta um fluxo com erro.
        /// </summary>
        /// <param name="action">Função a ser executada no caso de erro.</param>
        /// <returns>Res[TValue]</returns>
        public Res<TValue> Fail(Action<Error> action)
        {
            if (!IsError)
                return Value;

            return Try(ToFunc(action));
        }

        /// <summary>
        /// Converte Res[T] para uma tupla (Error, T)
        /// </summary>
        /// <param name="error">Error</param>
        /// <param name="value">TValue</param>
        public void Deconstruct(out Error error, out TValue value)
        {
            error = Error;
            value = Value.GetValue();
        }

        #region "Async"

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[Res[T]]</returns>
        public async Task<Res<T>> ThenAsync<T>(Func<TValue, T> function)
        {
            if (IsError)
                return await Task.FromResult(new Res<T>(Error));

            var val = Value.GetValueOrElse(default);
            if (IsSome)
                return await Task.Run(() => new Res<T>(function(val)));
            else
                return await Task.FromResult(new Res<T>(None.Instance));
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[Res[T]]</returns>
        public async Task<Res<T>> ThenAsync<T>(Func<TValue, Res<T>> function)
        {
            var val = Value.GetValueOrElse(default);
            if (IsError)
                return await Task.FromResult(new Res<T>(Error));
            else if (IsSome)
                return await Task.Run(() => function(val));
            else
                return await Task.FromResult(new Res<T>(None.Instance));
        }

        /// <summary>
        /// Executa a action e retorna Unit.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="action">Action a ser executada</param>
        /// <returns>Res[Unit]</returns>
        public async Task<Res<Unit>> ThenAsync(Action<TValue> action)
            => await ThenAsync(FunSharpUtils.ToFunc(action));

        #endregion


        private Func<Error> ToFunc(Action<Error> action)
        {
            var e = Error;
            return () => { action(e); return e; };
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

        /// <summary>
        /// Operador de cast implícito para Opt[TValue].
        /// </summary>
        /// <param name="opt">Objeto Opt[TValue]</param>
        public static implicit operator Res<TValue>(Opt<TValue> opt)
            => opt.IsSome ? Res.Of(opt.GetValue()) : new Res<TValue>(None.Instance);
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

        /// <summary>
        /// Transforma um Task[T] em um Task[Res[T]].
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="taskValue">Task do valor</param>
        /// <returns>Task[Res[T]]</returns>
        public static async Task<Res<T>> OfAsync<T>(Task<T> taskValue)
            => Of(await taskValue);
    }
}
