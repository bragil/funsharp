using System;

namespace FunSharp.Nullables
{
    public static class NullableExtensions
    {
        /// <summary>
        /// Converte um Nullable de T para um Opt de T.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="nullable">Nullable</param>
        /// <returns>Opt de T</returns>
        public static Opt<T> ToOptional<T>(this T? nullable) 
            where T : struct
            => nullable.HasValue ? nullable.Value : default;

        /// <summary>
        /// Aplica uma função no tipo Nullable e retorna um novo tipo Nullable.
        /// </summary>
        /// <typeparam name="U">Tipo do novo valor</typeparam>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="nullable">Objeto nullable</param>
        /// <param name="function">Função de transformação</param>
        /// <returns>Nullable de U</returns>
        public static U? Then<U, T>(this T? nullable, Func<T, U> function)
            where T : struct
            where U : struct
            => nullable.HasValue ? function(nullable.Value) : default;

        /// <summary>
        /// Aplica uma função no tipo Nullable e retorna um novo tipo Nullable.
        /// </summary>
        /// <typeparam name="U">Tipo do novo valor</typeparam>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="nullable">Objeto nullable</param>
        /// <param name="function">Função de transformação</param>
        /// <returns>Nullable de U</returns>
        public static U? Then<U, T>(this T? nullable, Func<T, U?> function)
            where T : struct
            where U : struct
            => nullable.HasValue ? function(nullable.Value) : default;

        /// <summary>
        /// Pattern matching do resultado.
        /// </summary>
        /// <typeparam name="U">Tipo do novo valor</typeparam>
        /// <typeparam name="T">Tipo do valor atual</typeparam>
        /// <param name="nullable">Objeto nullable</param>
        /// <param name="some">Função a ser executada caso haja valor</param>
        /// <param name="none">Função a ser executada se não houver valor</param>
        /// <returns>U</returns>
        public static U Match<U, T>(
                this T? nullable,
                Func<T, U> some,
                Func<None, U> none = null)
                where T : struct
                where U : class
            => nullable.HasValue ? some(nullable.Value) : none(None.Instance);
    }
}
