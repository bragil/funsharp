namespace FunSharp;

/// <summary>
/// Implementação de discriminated unions em C#.
/// </summary>
/// <typeparam name="T0"></typeparam>
/// <typeparam name="T1"></typeparam>
public readonly struct Choose<T0, T1>
{
    private readonly int index;
    private readonly T0 value0;
    private readonly T1 value1;

    private Choose(T0 value)
    {
        index = 0;
        value0 = value;
    }

    private Choose(T1 value)
    {
        index = 1;
        value1 = value;
    }

    /// <summary>
    /// Pattern matching do valor
    /// </summary>
    /// <typeparam name="T">Tipo do valor de retorno</typeparam>
    /// <param name="function0"></param>
    /// <param name="function1"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public T Match<T>(Func<T0, T> function0, Func<T1, T> function1)
        => index switch
        {
            0 => function0(value0),
            1 => function1(value1),
            _ => throw new ArgumentException($"Invalid index.")
        };

    public static implicit operator Choose<T0, T1>(T0 value) => new(value);
    public static implicit operator Choose<T0, T1>(T1 value) => new(value);
}

public readonly struct Choose<T0, T1, T2>
{
    private readonly int index;
    private readonly T0 value0;
    private readonly T1 value1;
    private readonly T2 value2;

    private Choose(T0 value)
    {
        index = 0;
        value0 = value;
    }

    private Choose(T1 value)
    {
        index = 1;
        value1 = value;
    }

    private Choose(T2 value)
    {
        index = 2;
        value2 = value;
    }

    public T Match<T>(
            Func<T0, T> function0, 
            Func<T1, T> function1,
            Func<T2, T> function2)
        => index switch
        {
            0 => function0(value0),
            1 => function1(value1),
            2 => function2(value2),
            _ => throw new ArgumentException($"Invalid index.")
        };

    public static implicit operator Choose<T0, T1, T2>(T0 value) => new(value);
    public static implicit operator Choose<T0, T1, T2>(T1 value) => new(value);
    public static implicit operator Choose<T0, T1, T2>(T2 value) => new(value);
}

public readonly struct Choose<T0, T1, T2, T3>
{
    private readonly int index;
    private readonly T0 value0;
    private readonly T1 value1;
    private readonly T2 value2;
    private readonly T3 value3;

    private Choose(T0 value)
    {
        index = 0;
        value0 = value;
    }

    private Choose(T1 value)
    {
        index = 1;
        value1 = value;
    }

    private Choose(T2 value)
    {
        index = 2;
        value2 = value;
    }

    private Choose(T3 value)
    {
        index = 3;
        value3 = value;
    }

    public T Match<T>(
            Func<T0, T> function0,
            Func<T1, T> function1,
            Func<T2, T> function2,
            Func<T3, T> function3)
        => index switch
        {
            0 => function0(value0),
            1 => function1(value1),
            2 => function2(value2),
            3 => function3(value3),
            _ => throw new ArgumentException($"Invalid index.")
        };

    public static implicit operator Choose<T0, T1, T2, T3>(T0 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3>(T1 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3>(T2 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3>(T3 value) => new(value);
}

public readonly struct Choose<T0, T1, T2, T3, T4>
{
    private readonly int index;
    private readonly T0 value0;
    private readonly T1 value1;
    private readonly T2 value2;
    private readonly T3 value3;
    private readonly T4 value4;

    private Choose(T0 value)
    {
        index = 0;
        value0 = value;
    }

    private Choose(T1 value)
    {
        index = 1;
        value1 = value;
    }

    private Choose(T2 value)
    {
        index = 2;
        value2 = value;
    }

    private Choose(T3 value)
    {
        index = 3;
        value3 = value;
    }

    private Choose(T4 value)
    {
        index = 4;
        value4 = value;
    }

    public T Match<T>(
            Func<T0, T> function0,
            Func<T1, T> function1,
            Func<T2, T> function2,
            Func<T3, T> function3,
            Func<T4, T> function4)
        => index switch
        {
            0 => function0(value0),
            1 => function1(value1),
            2 => function2(value2),
            3 => function3(value3),
            4 => function4(value4),
            _ => throw new ArgumentException($"Invalid index.")
        };

    public static implicit operator Choose<T0, T1, T2, T3, T4>(T0 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3, T4>(T1 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3, T4>(T2 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3, T4>(T3 value) => new(value);
    public static implicit operator Choose<T0, T1, T2, T3, T4>(T4 value) => new(value);
}