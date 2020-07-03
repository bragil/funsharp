using System;
using System.Net;
using System.Threading.Tasks;

namespace FunSharp.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Executa a função e retorna Task[TNew].
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="TResult">Tipo do valor atual</typeparam>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[TNew]</returns>
        public static async Task<TNew> Then<TResult, TNew>(this Task<TResult> source, Func<TResult, TNew> function)
            => function(await source);


        /// <summary>
        /// Executa a função e retorna Task[TNew].
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[TNew]</returns>
        public static async Task<TNew> Then<TResult, TNew>(this Task<TResult> source, Func<TResult, Task<TNew>> function)
            => await function(await source);

        /// <summary>
        /// Executa a action e retorna Task.
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <param name="action">Action a ser executada</param>
        /// <returns>Task</returns>
        public static async Task Then<TResult>(this Task<TResult> source, Action<TResult> action)
            => action(await source);


        /// <summary>
        /// Executa a função e retorna Task[Res[TNew]].
        /// Possibilita o encadeamento de execuções.
        /// </summary>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Task[Res[TNew]]</returns>
        public static async Task<Res<TNew>> Then<TResult, TNew>(
            this Task<TResult> source, 
            Func<TResult, Res<TNew>> function)
            => function(await source);

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="TResult">Tipo do valor atual</typeparam>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="source">Task atual</param>
        /// <param name="some">Função a ser executada se houver valor</param>
        /// <param name="none">Função a ser executada se não houver valor</param>
        /// <returns>Task[TNew]</returns>
        public static async Task<TResult> Match<TResult>(
            this Task<TResult> source,
            Func<TResult, TResult> some,
            Func<None, TResult> none)
        {
            Opt<TResult> opt = await source;

            return opt.Match(some, none);
        }

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="TResult">Tipo do valor atual</typeparam>
        /// <typeparam name="TNew">Tipo do valor de retorno</typeparam>
        /// <param name="source">Task atual</param>
        /// <param name="some">Função a ser executada se houver valor</param>
        /// <param name="none">Função a ser executada se não houver valor</param>
        /// <returns>Task[TNew]</returns>
        public static async Task<TNew> Match<TResult, TNew>(
            this Task<TResult> source,
            Func<TResult, TNew> some,
            Func<None, TNew> none)
        {
            Opt<TResult> opt = await source;

            return opt.Match(some, none);
        }

    }
}
