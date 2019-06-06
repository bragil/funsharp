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
        /// <param name="errorData">Qualquer objeto relacionado ao erro.</param>
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
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <param name="errorData">Qualquer objeto relacionado ao erro.</param>
        /// <returns>Task[Res[T]]</returns>
        public static async Task<Res<T>> TryAsync<T>(Func<Task<T>> function, string errorMessage = null, object errorData = null)
        {
            try
            {
                return await Res.OfAsync(function());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Error(ex));
            }
        }

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Res[Unit]</returns>
        public static Res<Unit> Try(Action action, string errorMessage = null, object errorData = null)
            => Try(FunSharpUtils.ToFunc(action), errorMessage, errorData);

        /// <summary>
        /// Encapsula o bloco try..catch.
        /// </summary>
        /// <param name="function">Função a ser executada</param>
        /// <param name="errorMessage">Mensagem customizada de erro (opcional)</param>
        /// <returns>Task[Res[Unit]]</returns>
        public static async Task<Res<Unit>> TryAsync(Action action, string errorMessage = null, object errorData = null)
        {
            try
            {
                action();
                return await Task.FromResult(Unit.Instance);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Error(ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disp"></param>
        /// <param name="function"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorData"></param>
        /// <returns></returns>
        public static Res<T> Use<T, TDisp>(TDisp disp, Func<TDisp, T> function, string errorMessage = null, object errorData = null)
            where TDisp: IDisposable
        {
            try
            {
                return function(disp);
            }
            catch (Exception ex)
            {
                return new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
            }
            finally
            {
                disp?.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disp"></param>
        /// <param name="function"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errorData"></param>
        /// <returns></returns>
        public static Res<Unit> Use<TResource>(TResource disp, Action<TResource> function, string errorMessage = null, object errorData = null)
            where TResource : IDisposable
        {
            try
            {
                function(disp);
                return Unit.Instance;
            }
            catch (Exception ex)
            {
                return new Error(exception: ex, message: errorMessage ?? ex.Message, errorData: errorData);
            }
            finally
            {
                disp?.Dispose();
            }
        }
    }
}
