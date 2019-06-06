using System;

namespace FunSharp
{
    public static class FunSharpUtils
    {
        /// <summary>
        /// Converte um Action[T] para um Func[T, Unit].
        /// </summary>
        /// <typeparam name="T">Tipo do parâmetro do Action</typeparam>
        /// <param name="action">Action[T]</param>
        /// <returns>Func[T, Unit]</returns>
        public static Func<T, Unit> ToFunc<T>(Action<T> action)
            => parm => { action(parm); return Unit.Instance; };

        /// <summary>
        /// Converte um Action[T] para um Func[T, Unit].
        /// </summary>
        /// <typeparam name="T">Tipo do parâmetro do Action</typeparam>
        /// <param name="action">Action[T]</param>
        /// <returns>Func[T, Unit]</returns>
        public static Func<Unit> ToFunc(Action action)
            => () => { action(); return Unit.Instance; };
    }
}
