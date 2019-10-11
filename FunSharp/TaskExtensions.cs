using System;
using System.Threading.Tasks;

namespace FunSharp
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="TResult">Tipo do valor atual</typeparam>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[T]</returns>
        public static async Task<TNew> Then<TResult, TNew>(this Task<TResult> source, Func<TResult, TNew> function)
        {
            TResult t = await source;
            return function(t);
        }

        /// <summary>
        /// Executa a função e retorna o resultado.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[T]</returns>
        public static async Task<TNew> Then<TResult, TNew>(this Task<TResult> source, Func<TResult, Task<TNew>> function)
        {
            return await function(await source);
        }

        /// <summary>
        /// Executa a action e retorna Unit.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="action">Action a ser executada</param>
        /// <returns>Task</returns>
        public static async Task Then<TResult>(this Task<TResult> source, Action<TResult> action)
        {
            TResult t = await source;
            action(t);
        }

    }
}
