namespace Unit.Lib.Core.DomainModel
{
    public class UnitBaseName<S, T> : NameSymbolable where S : IScalar<T>
    {
        public UnitDimension Dimension { get; set; }

        public S Factor { get; set; }

        public string AsString => Symbol;

        public string AsAsciiString => AsciiSymbol;
    }
}