namespace Unit.Lib.Core.DomainModel
{
    public class UnitPrefix<S, T> : NameSymbolable where S : IScalar<T>
    {
        public bool Invert { get; set; }

        public S Factor { get; set; }

        public string AsString => Symbol;

        public string AsAsciiString => AsciiSymbol;
    }
}