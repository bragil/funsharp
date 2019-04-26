using System;

namespace FunSharp
{
    public static class TryFunctions
    {
        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Error</returns>
        public static Error Try(Func<Error> function)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                return new Error(exception: ex);
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Res[T]</returns>
        public static Res<T> Try<T>(Func<T> function, string errorMessage = null)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                return new Error(exception: ex, message: errorMessage);
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Res[Unit]</returns>
        public static Res<Unit> Try(Action action, string errorMessage = null)
            => Try(Utils.ToFunc(action), errorMessage);
    }
}
