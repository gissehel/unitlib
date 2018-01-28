namespace Unit.Lib.Core.DomainModel
{
    public class Symbolable
    {
        public string Symbol { get; set; }

        private string _asciiSymbol = null;

        public string AsciiSymbol
        {
            get => _asciiSymbol ?? Symbol;
            set => _asciiSymbol = value;
        }
    }
}