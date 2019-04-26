using System;
using System.Threading.Tasks;

namespace FunSharp
{
    public static class TryFunctions
    {
        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <returns>Error</returns>
        internal static Error Try(Func<Error> function)
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
        public static Res<T> Try<T>(Func<T> function, string errorMessage = null, object errorData = null)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                return new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch (assíncrono).
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Task[Res[T]]</returns>
        public async static Task<Res<T>> TryAsync<T>(Func<T> function, string errorMessage = null, object errorData = null)
            => await Task.Run(() => Try(function, errorMessage, errorData));

        /// <summary>
        /// Encapsula o bloco try..catch (assíncrono).
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Task[Res[T]]</returns>
        public async static Task<Res<T>> TryAsync<T>(Func<Task<T>> function, string errorMessage = null, object errorData = null)
            => await Task.Run(async () =>
            {
                try
                {
                    T value = await function();
                    return new Res<T>(value);
                }
                catch (Exception ex)
                {
                    return new Res<T>(new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData));
                }

            });

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Res[Unit]</returns>
        public static Res<Unit> Try(Action action, string errorMessage = null, object errorData = null)
            => Try(Utils.ToFunc(action), errorMessage, errorData);

        /// <summary>
        /// Encapsula o bloco try..catch (assíncrono).
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Res[Unit]</returns>
        public async static Task<Res<Unit>> TryAsync(Action action, string errorMessage = null, object errorData = null)
            => await Task.Run(() => Try(Utils.ToFunc(action), errorMessage, errorData));

    }
}
