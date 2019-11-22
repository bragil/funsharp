using System;
using System.Threading.Tasks;
using static FunSharp.TryFunctions;

namespace FunSharp
{
    /// <summary>
    /// Resultado de uma computação com retorno de valor.
    /// </summary>
    /// <typeparam name="TValue">Tipo do objeto de valor</typeparam>
    public class Res<TValue>
    {
        private readonly Error error;
        private Opt<TValue> value;

        public bool IsError { get; }
        public bool IsSome { get; }
        public bool IsNone { get; }

        internal Res(TValue value)
        {
            this.value = Opt.Of(value);
            error = default;
            IsError = false;
            IsNone = this.value.IsNone;
            IsSome = this.value.IsSome;
        }

        internal Res(Error error)
        {
            value = Opt.Empty<TValue>();
            this.error = error;
            IsError = true;
            IsNone = false;
            IsSome = false;
        }

        internal Res(None none)
        {
            value = Opt.Empty<TValue>();
            error = default;
            IsError = false;
            IsNone = value.IsNone;
            IsSome = value.IsSome;
        }

        /// <summary>
        /// Tenta obter o valor. Em caso de erro, retorna o fallback informado.
        /// </summary>
        /// <param name="fallback">Valor a ser retornado em caso de erro.</param>
        /// <returns>Valor</returns>
        public TValue GetValueOrElse(TValue fallback)
            => IsError ? fallback : value.GetValueOrElse(fallback);

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="some">Função a ser executada em execuções bem-sucedidas com valor de retorno.</param>
        /// <param name="error">Função a ser executada em execuções com erro.</param>
        /// <param name="none">Função a ser executada em execuções bem-sucedidas que não retornam valor.</param>
        /// <returns>T</returns>
        public T Match<T>(Func<TValue, T> some, Func<Error, T> error, Func<None, T> none = null)
            => IsError ? error(this.error) : value.Match(some, none);

        /// <summary>
        /// Pattern matching do resultado, obtém o valor do resultado, se houver (IsSome).
        /// Se houve algum erro (IsError), retorna o resultado da função error.
        /// Se não retornou nenhum valor (IsNone), retorna o resultado da função none.
        /// </summary>
        /// <param name="error">Função a ser executada em execuções com erro.</param>
        /// <param name="none">Função a ser executada em execuções bem-sucedidas que não retornam valor (opcional).</param>
        /// <returns></returns>
        public TValue Match(Func<Error, TValue> error, Func<None, TValue> none = null)
        {
            if (IsError)
                return error(this.error);
            else if (IsSome)
                return value.GetValue();
            else
                return none(None.Instance);
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
            if (IsError) return error;

            var val = value.GetValueOrElse(default);
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
                return error;
            else
                return IsSome ? function(value.GetValueOrElse(default)) : None.Instance;
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
        /// <param name="condition">Função de condição (predicado)</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[T]</returns>
        public Res<T> IfThen<T>(Predicate<TValue> condition, Func<TValue, T> isTrue, Func<TValue, T> isFalse)
            => condition(value.GetValue()) ? Then(isTrue) : Then(isFalse);

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Função de condição (predicado)</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[T]</returns>
        public Res<T> IfThen<T>(Predicate<TValue> condition, Func<TValue, Res<T>> isTrue, Func<TValue, Res<T>> isFalse)
            => condition(value.GetValue()) ? Then(isTrue) : Then(isFalse);

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Função de condição (predicado)</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[Unit]</returns>
        public Res<Unit> IfThen(Predicate<TValue> condition, Action<TValue> isTrue, Action<TValue> isFalse)
            => condition(value.GetValue()) ? Then(isTrue) : Then(isFalse);

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
                return value;

            return Try(ToFunc(action));
        }

        /// <summary>
        /// Converte Res[T] para uma tupla (Error, T)
        /// </summary>
        /// <param name="error">Error</param>
        /// <param name="value">TValue</param>
        public void Deconstruct(out Error error, out TValue value)
        {
            error = this.error;
            value = this.value.GetValue();
        }


        private Func<Error> ToFunc(Action<Error> action)
        {
            var e = error;
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

    /// <summary>
    /// Resultado de uma computação sem retorno de valor, indicando apenas sucesso ou falha.
    /// </summary>
    public class Res
    {
        private readonly Error error;

        public bool IsError { get; }

        public bool IsSuccess => !IsError;

        /// <summary>
        /// Construtor para execuções bem-sucedidas.
        /// </summary>
        internal Res()
            => IsError = false;

        /// <summary>
        /// Construtor para execuções com erro.
        /// </summary>
        internal Res(Error error)
        {
            this.error = error;
            IsError = true;
        }

        /// <summary>
        /// Pattern matching do resultado.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="success">Função a ser executada em caso de sucesso</param>
        /// <param name="error">Função a ser executada em caso de erro</param>
        /// <returns>Resultado de uma das funções passadas</returns>
        public T Match<T>(Func<T> success, Func<Error, T> error)
            => IsSuccess ? success() : error(this.error);

        /// <summary>
        /// Resultado bem-sucedido de uma execução.
        /// </summary>
        /// <returns>Res</returns>
        public static Res Success() => new Res();

        /// <summary>
        /// Resultado de uma execução com erro.
        /// </summary>
        /// <param name="error">Objeto de erro</param>
        /// <returns>Res</returns>
        public static Res Error(Error error) => new Res(error);

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

        /// <summary>
        /// Operador de cast implícito para erro.
        /// </summary>
        /// <param name="error">Objeto de erro</param>
        public static implicit operator Res(Error error)
            => new Res(error);
    }
}
