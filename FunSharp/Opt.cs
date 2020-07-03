using System;

namespace FunSharp
{
    /// <summary>
    /// Envolve um possível valor que pode existir ou não. 
    /// </summary>
    /// <typeparam name="TValue">Tipo do valor</typeparam>
    public class Opt<TValue>
    {
        /// <summary>
        /// O valor
        /// </summary>
        private TValue Value { get; set; }

        /// <summary>
        /// Se existir valor será true;
        /// </summary>
        public bool IsSome { get; }

        /// <summary>
        /// Se não existir valor será true.
        /// </summary>
        public bool IsNone { get; }

        /// <summary>
        /// Construtor, recebe o valor.
        /// </summary>
        /// <param name="some">Valor</param>
        public Opt(TValue some)
        {
            Value = some;
            IsNone = Value == null || Value.Equals(default(TValue));
            IsSome = !IsNone;
        }

        /// <summary>
        /// Construtor para ausência de valor (None).
        /// </summary>
        /// <param name="_"></param>
        public Opt(None _)
        {
            Value = default;
            IsNone = true;
            IsSome = false;
        }

        /// <summary>
        /// Retorna o valor, se houver.
        /// </summary>
        /// <returns>Valor</returns>
        internal TValue GetValue()
            => Value;

        /// <summary>
        /// Tenta obter o valor. Se não existir (None), retorna o fallback informado.
        /// </summary>
        /// <param name="fallback">Valor a ser retornado em caso de None.</param>
        /// <returns>Valor</returns>
        public TValue GetValueOrElse(TValue fallback)
            => IsNone ? fallback : Value;

        /// <summary>
        /// Caso haja valor, executa a função passada. Retorna a instância do objeto Opt atual.
        /// </summary>
        /// <param name="action">Função a ser executada caso haja valor</param>
        /// <returns>Opt[TValue]</returns>
        public Opt<TValue> OnSome(Action<TValue> action)
        {
            if (IsSome)
                action(Value);

            return this;
        }

        /// <summary>
        /// Caso não haja valor (None), executa a função passada. Retorna a instância do objeto Opt atual.
        /// </summary>
        /// <param name="action">Função a ser executada caso não haja valor (None)</param>
        /// <returns>Opt[TValue]</returns>
        public Opt<TValue> OnNone(Action action)
        {
            if (IsNone)
                action();

            return this;
        }

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="some">Função a ser executada em execuções bem-sucedidas com valor de retorno.</param>
        /// <param name="none">Função a ser executada em execuções bem-sucedidas que não retornam valor.</param>
        /// <returns>T</returns>
        public T Match<T>(Func<TValue, T> some, Func<None, T> none = null)
        {
            if (IsSome)
                return some(Value);
            else
                return none != null ? none(None.Instance) : default;
        }

        /// <summary>
        /// Aplica uma função sobre o valor e retorna um Opt do novo valor.
        /// </summary>
        /// <typeparam name="T">Tipo do novo valor</typeparam>
        /// <param name="function">Função</param>
        /// <returns>Opt do novo valor</returns>
        public Opt<T> Then<T>(Func<TValue, T> function)
        => IsSome
            ? function(Value)
            : default;

        /// <summary>
        /// Aplica uma função sobre o valor e retorna um Opt do novo valor.
        /// </summary>
        /// <typeparam name="T">Tipo do novo valor</typeparam>
        /// <param name="function">Função</param>
        /// <returns>Opt do novo valor</returns>
        public Opt<T> Then<T>(Func<TValue, Opt<T>> function)
            => IsSome
                ? function(Value)
                : default;

        /// <summary>
        /// Operador de cast implícito para valor (Some).
        /// </summary>
        /// <param name="value">Valor</param>
        public static implicit operator Opt<TValue>(TValue value)
            => new Opt<TValue>(value);

        /// <summary>
        /// Operador de cast implícito para None (nenhum valor).
        /// </summary>
        /// <param name="none">Objeto None</param>
        public static implicit operator Opt<TValue>(None _)
            => new Opt<TValue>(None.Instance);
    }

    public static class Opt
    {
        /// <summary>
        /// Envolve um valor em um Opt.
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="value">Valor</param>
        /// <returns>Opt[T]</returns>
        public static Opt<T> Of<T>(T value)
            => new Opt<T>(value);

        /// <summary>
        /// Retorna um None
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <returns>None</returns>
        public static Opt<T> Empty<T>()
            => new Opt<T>(None.Instance);
    }
}
