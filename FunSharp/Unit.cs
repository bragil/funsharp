namespace FunSharp
{
    /// <summary>
    /// Tipo que representa o retorno de funções que não retornam valor. Substituto para void.
    /// </summary>
    public class Unit
    {
        public static Unit Instance => new Unit();
    }
}
