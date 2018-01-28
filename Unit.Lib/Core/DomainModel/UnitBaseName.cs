namespace Unit.Lib.Core.DomainModel
{
    public class UnitBaseName<T> : NameSymbolable
    {
        public UnitDimension Dimension { get; set; }

        public T Factor { get; set; }

        public string AsString => Symbol;

        public string AsAsciiString => AsciiSymbol;
    }

    public class UnitBaseName : UnitBaseName<float> { }
}