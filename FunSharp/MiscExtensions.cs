using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
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
                   withValue: v => Task.FromResult(v),
                   withError: e => Task.FromException<T>(e.Exception),
                   noValue: () => Task.FromResult(default(T))
               );

        /// <summary>
        /// Converte uma Task[T] em um Res[T].
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="task">Task[T]</param>
        /// <returns>Res[T]</returns>
        public static Res<T> FromTask<T>(this Task<T> task)
        {
            if (task.IsFaulted)
                return new Error(task.Exception);
            else
                return task.Result;
        }

        /// <summary>
        /// Converte um Res[T] em um IObservable[T] (Rx.Net).
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="res">Res[T]</param>
        /// <returns>IObservable[T]</returns>
        public static IObservable<T> ToObservable<T>(this Res<T> res)
            => res.Match(
                   withValue: v => Observable.Return(v),
                   withError: e => Observable.Throw<T>(e.Exception),
                   noValue: () => Observable.Empty<T>()
               );

        /// <summary>
        /// Converte o Res[T] em uma tupla (Error, T).
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="res">Res[T]</param>
        /// <returns>Tupla (Error, T)</returns>
        public static (Error Error, T Value) ToTuple<T>(this Res<T> res)
            => res.Match(
                   withValue: v => (default(Error), v),
                   withError: e => (e, default(T)),
                   noValue: () => (default(Error), default(T))
               );
    }
}
