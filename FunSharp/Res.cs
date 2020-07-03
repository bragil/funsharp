using System;
using System.Threading.Tasks;
using static FunSharp.TryFunctions;

namespace FunSharp
{
    /// <summary>
    /// Resultado de uma computação com retorno de valor.
    /// </summary>
    /// <typeparam name="TValue">Tipo do objeto de valor</typeparam>
    public struct Res<TValue>
    {
        private readonly Error error;
        private readonly Opt<TValue> value;

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

        internal Res(None _)
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
        /// Tenta obter o valor. Em caso de erro ou inexistência de valor, 
        /// executa suas respectivas funções passadas.
        /// </summary>
        /// <param name="errorFunction">Função a ser executada em caso de erro</param>
        /// <param name="noneFunction">Função a ser executada em caso de inexistência de valor</param>
        /// <returns>Valor</returns>
        public TValue GetValueOrMatch(Func<Error, TValue> errorFunction, Func<None, TValue> noneFunction)
        {
            if (IsSome)
                return value.GetValue();
            else if (IsError)
                return errorFunction(error);
            else
                return noneFunction(None.Instance);
        }

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="some">Função a ser executada em execuções bem-sucedidas com valor de retorno.</param>
        /// <param name="error">Função a ser executada em execuções com erro.</param>
        /// <param name="none">Função a ser executada em execuções bem-sucedidas que não retornam valor.</param>
        /// <returns>T</returns>
        public T Match<T>(
                Func<TValue, T> some, 
                Func<Error, T> error, 
                Func<None, T> none = null)
            => IsError ? error(this.error) : value.Match(some, none);

        /// <summary>
        /// Em caso de erro, executa a função passada.
        /// </summary>
        /// <param name="function">Função a ser executada em caso de erro</param>
        /// <returns>Instância atual</returns>
        public Res<TValue> OnError(Action<Error> function)
        {
            if (IsError)
                function(error);

            return this;
        }

        /// <summary>
        /// Em caso de execução bem-sucedida com retorno de valor, executa a função passada.
        /// </summary>
        /// <param name="someFunction">Função a ser executada em caso retorno de valor</param>
        /// <param name="noneFunction">Função a ser executada em caso de não retornar valor (None)</param>
        /// <returns>Instância atual</returns>
        public Res<TValue> OnSuccess(Action<TValue> someFunction, Action noneFunction = null)
        {
            try
            {
                if (IsSome)
                    someFunction(value.GetValue());
                else
                    noneFunction?.Invoke();
            }
            catch (Exception ex)
            {
                return new Error(ex, ex.Message);
            }

            return this;
        }

        /// <summary>
        /// Combina o objeto do resultado passado, se não houver erro no resultado atual.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="res">Objeto do resultado</param>
        /// <returns>Novo resultado passado</returns>
        public Res<T> Combine<T>(Res<T> res)
        {
            if (IsError)
                return error;

            return res;
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
            if (IsError) 
                return error;
            else if (IsSome)
            {
                try
                {
                    return function(value.GetValue());
                }
                catch (Exception ex)
                {
                    return new Error(ex, ex.Message);
                }
            }
            else
                return None.Instance;
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
            else if (IsSome)
            {
                try
                {
                    return function(value.GetValue());
                }
                catch (Exception ex)
                {
                    return new Error(ex, ex.Message);
                }
            }
            return None.Instance;
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Res</returns>
        public Res Then(Action<TValue> function)
        {
            if (IsError)
                return error;
            else
            {
                try
                {
                    function(value.GetValue());
                    return Res.Success();
                }
                catch (Exception ex)
                {
                    return new Error(ex, ex.Message);
                }
            }
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Res</returns>
        public Res Then(Action function)
        {
            if (IsError)
                return error;
            else
            {
                try
                {
                    function();
                    return Res.Success();
                }
                catch (Exception ex)
                {
                    return new Error(ex, ex.Message);
                }
            }
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função assíncrona a ser executada</param>
        /// <returns>Task[Res[T]]</returns>
        public async Task<Res<T>> ThenAsync<T>(Func<TValue, Task<T>> function)
        {
            if (IsError) return error;

            var val = value.GetValueOrElse(default);

            return IsSome ? await TryAsync(() => function(val), errFunction: null) : None.Instance;
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função assíncrona a ser executada</param>
        /// <returns>Task[Res[T]]</returns>
        public async Task<Res<T>> ThenAsync<T>(Func<TValue, Task<Res<T>>> function)
        {
            if (IsError)
                return error;
            else
                return IsSome ? await function(value.GetValueOrElse(default)) : None.Instance;
        }


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

        /////// <summary>
        /////// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /////// </summary>
        /////// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /////// <param name="condition">Função de condição (predicado)</param>
        /////// <param name="isTrue">Função a ser executada se condition = true</param>
        /////// <param name="isFalse">Função a ser executada se condition = false</param>
        /////// <returns>Res[Unit]</returns>
        ////public Res<Unit> IfThen(Predicate<TValue> condition, Action<TValue> isTrue, Action<TValue> isFalse)
        ////    => condition(value.GetValue()) ? Then(isTrue) : Then(isFalse);

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

        /////// <summary>
        /////// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /////// </summary>
        /////// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /////// <param name="condition">Condição</param>
        /////// <param name="isTrue">Função a ser executada se condition = true</param>
        /////// <param name="isFalse">Função a ser executada se condition = false</param>
        /////// <returns>Res[Unit]</returns>
        ////public Res<Unit> IfThen(bool condition, Action<TValue> isTrue, Action<TValue> isFalse)
        ////    => condition ? Then(isTrue) : Then(isFalse);

        /////// <summary>
        /////// Intercepta um fluxo com erro.
        /////// </summary>
        /////// <param name="action">Função a ser executada no caso de erro.</param>
        /////// <returns>Res[TValue]</returns>
        ////public Res<TValue> Fail(Action<Error> action)
        ////{
        ////    if (!IsError)
        ////        return value;
            
        ////    return Try(ToFunc(action));
        ////}

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

        /// <summary>
        /// Converte Res[T] em Res (despreza retorno de valor).
        /// </summary>
        /// <returns>Res</returns>
        public Res ToRes()
            => IsError ? Res.Error(error) : Res.Success();


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
    public struct Res
    {
        private readonly Error error;

        public bool IsError { get; }

        public bool IsSuccess => !IsError;

        /// <summary>
        /// Construtor para execuções bem-sucedidas.
        /// </summary>
        internal Res(bool _)
        {
            error = null;
            IsError = false;
        }

        /// <summary>
        /// Construtor para execuções com erro.
        /// </summary>
        internal Res(Error error)
        {
            this.error = error;
            IsError = true;
        }

        /// <summary>
        /// Em caso de erro, executa a função passada.
        /// </summary>
        /// <param name="function">Função a ser executada em caso de erro</param>
        /// <returns>Instância atual</returns>
        public Res OnError(Action<Error> function)
        {
            if (IsError)
                function(error);

            return this;
        }

        /// <summary>
        /// Em caso de sucesso, executa a função passada.
        /// </summary>
        /// <param name="function">Função a ser executada em caso de sucesso</param>
        /// <returns>Instância atual</returns>
        public Res OnSuccess(Action function)
        {
            if (IsSuccess)
                function();

            return this;
        }

        /// <summary>
        /// Retorna o Res passado, se não houver erro no Res atual.
        /// </summary>
        /// <param name="res">Novo resultado</param>
        /// <returns>Res</returns>
        public Res Combine(Res res)
        {
            if (IsError)
                return error;

            return res;
        }

        /// <summary>
        /// Retorna o Res[T] passado, se não houver erro no Res atual.
        /// </summary>
        /// <param name="res">Novo resultado</param>
        /// <returns>Res[T]</returns>
        public Res<T> Combine<T>(Res<T> res)
        {
            if (IsError)
                return error;

            return res;
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
        public static Res Success() => new Res(true);

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
