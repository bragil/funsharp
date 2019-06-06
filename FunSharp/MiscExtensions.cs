using System;
using System.Threading.Tasks;

namespace FunSharp
{
    /// <summary>
    /// Métodos de extensão 
    /// </summary>
    public static class MiscExtensions
    {
        /// <summary>
        /// Converte o Res[T] em Task[T].
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="res">Res[T]</param>
        /// <returns>Task[T]</returns>
        public static Task<T> ToTask<T>(this Res<T> res)
            => res.Match(
                   some:  v => Task.FromResult(v),
                   error: e => Task.FromException<T>(e.Exception),
                   none:  _ => Task.FromResult(default(T))
               );

        /// <summary>
        /// Converte uma Task[T] em um Res[T].
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="task">Task[T]</param>
        /// <returns>Res[T]</returns>
        public static Res<T> FromTask<T>(this Task<T> task)
        {
            try
            {
                if (task.IsFaulted)
                    return new Error(task.Exception);
                else
                    return task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new Error(ex);
            }
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[Res[T]]</returns>
        public static async Task<Res<T>> ThenAsync<TValue, T>(this Task<Res<TValue>> task, Func<TValue, T> function)
        {
            var res = await task;
            return await res.ThenAsync(function);
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[Res[T]]</returns>
        public static async Task<Res<T>> ThenAsync<TValue, T>(this Task<Res<TValue>> task, Func<TValue, Res<T>> function)
        {
            var res = await task;
            return await res.ThenAsync(function);
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[Res[Unit]]</returns>
        public static async Task<Res<Unit>> ThenAsync<T>(this Task<Res<T>> task, Action<T> function)
        {
            var res = await task;
            return await res.ThenAsync(function);
        }

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Condição</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[T]</returns>
        public static async Task<Res<T>> IfThenAsync<TValue, T>(this Task<Res<TValue>> task, bool condition, Func<TValue, Res<T>> isTrue, Func<TValue, Res<T>> isFalse)
            => condition ? await task.ThenAsync(isTrue) : await task.ThenAsync(isFalse);

        /// <summary>
        /// Se condition for true, executa a função isTrue, senão, executa isFalse.
        /// </summary>
        /// <typeparam name="T">Tipo do valor de retorno</typeparam>
        /// <param name="condition">Condição</param>
        /// <param name="isTrue">Função a ser executada se condition = true</param>
        /// <param name="isFalse">Função a ser executada se condition = false</param>
        /// <returns>Res[Unit]</returns>
        public static async Task<Res<Unit>> IfThenAsync<T>(this Task<Res<T>> task, bool condition, Action<T> isTrue, Action<T> isFalse)
            => condition ? await task.ThenAsync(isTrue) : await task.ThenAsync(isFalse);
    }
}
