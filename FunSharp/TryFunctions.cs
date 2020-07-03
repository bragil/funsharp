using System;
using System.Threading.Tasks;

namespace FunSharp
{
    public static class TryFunctions
    {
        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errFunction">Função para tratamento do erro (opcional)</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <param name="errorData">Qualquer objeto relacionado ao erro.</param>
        /// <returns>Res[T]</returns>
        public static Res<T> Try<T>(
            Func<T> function, 
            Func<Error, Error> errFunction = null, 
            string errorMessage = null, 
            object errorData = null)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                var error = new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
                if (errFunction == null)
                    return error;

                return errFunction(error);
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errFunction">Função para tratamento do erro (opcional)</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <param name="errorData">Qualquer objeto relacionado ao erro.</param>
        /// <returns>Res[T]</returns>
        public static Res<T> Try<T>(
            Func<Res<T>> function,
            Func<Error, Error> errFunction = null,
            string errorMessage = null,
            object errorData = null)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                var error = new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
                if (errFunction == null)
                    return error;

                return errFunction(error);
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errFunction">Função para tratamento do erro (opcional)</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <param name="errorData">Qualquer objeto relacionado ao erro.</param>
        /// <returns>Task[Res[T]]</returns>
        public static async Task<Res<T>> TryAsync<T>(
            Func<Task<T>> function, 
            Func<Error, Error> errFunction = null, 
            string errorMessage = null, 
            object errorData = null)
        {
            try
            {
                return await function();
            }
            catch (Exception ex)
            {
                var error = new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
                if (errFunction == null)
                    return error;

                return errFunction(error);
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errFunction">Função para tratamento do erro (opcional)</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <param name="errorData">Qualquer objeto relacionado ao erro.</param>
        /// <returns>Task[Res[T]]</returns>
        public static async Task<Res> TryAsync(
            Func<Task<Res>> function, 
            Func<Error, Error> errFunction = null, 
            string errorMessage = null, 
            object errorData = null)
        {
            try
            {
                return await function();
            }
            catch (Exception ex)
            {
                var error = new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
                if (errFunction == null)
                    return error;

                return errFunction(error);
            }
        }
    }
}
