namespace FunSharp
{
    /// <summary>
    /// Tipo que representa ausência de valor. Substituto para null.
    /// </summary>
    public struct None
    {
        public static None Instance => new None();
    }
}
