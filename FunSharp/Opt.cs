using System;

namespace FunSharp
{
    public struct Opt<TValue>
    {
        private TValue Value { get; set; }
        public bool IsSome { get; }
        public bool IsNone { get; }

        public Opt(TValue some)
        {
            Value = some;
            IsNone = Value == null || (Value.Equals(default(TValue)) && Value.GetType() != typeof(Unit));
            IsSome = !IsNone;
        }

        public Opt(None none)
        {
            Value = default;
            IsNone = true;
            IsSome = false;
        }

        public TValue GetValue()
            => Value;

        /// <summary>
        /// Tenta obter o valor. Se não existir (None), retorna o fallback informado.
        /// </summary>
        /// <param name="fallback">Valor a ser retornado em caso de None.</param>
        /// <returns>Valor</returns>
        public TValue GetValueOrElse(TValue fallback)
            => IsNone ? fallback : Value;

        /// <summary>
        /// Pattern matching do resultado, possibilita obter o valor.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="some">Função a ser executada em execuções bem-sucedidas com valor de retorno.</param>
        /// <param name="none">Função a ser executada em execuções bem-sucedidas que não retornam valor.</param>
        /// <returns>T</returns>
        public T Match<T>(Func<TValue, T> some, Func<None, T> none = null)
            => IsSome 
                ? some(Value) 
                : none(None.Instance);


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
        public static implicit operator Opt<TValue>(None none)
            => new Opt<TValue>(None.Instance);
    }

    public struct Option
    {
        public static Opt<T> Of<T>(T value)
            => new Opt<T>(value);

        public static Opt<T> Empty<T>()
            => new Opt<T>(None.Instance);
    }
}
