namespace Unit.Lib.Core.DomainModel
{
    public class UnitPrefix<T> : NameSymbolable
    {
        public bool Invert { get; set; }

        public T Factor { get; set; }

        public string AsString => Symbol;

        public string AsAsciiString => AsciiSymbol;
    }

    public class UnitPrefix : UnitPrefix<long> { }
}