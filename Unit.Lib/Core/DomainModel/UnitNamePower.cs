namespace Unit.Lib.Core.DomainModel
{
    public class UnitNamePower<S, T> where S : IScalar<T>
    {
        public UnitNamePower(UnitName<S, T> unitName, long power)
        {
            UnitName = unitName;
            Power = power;
        }

        public UnitNamePower(UnitName<S, T> unitName) : this(unitName, 1)
        {
        }

        public UnitNamePower(UnitPrefix<S, T> prefix, UnitBaseName<S, T> baseName, long power) : this(new UnitName<S, T>(prefix, baseName), power)
        {
        }

        public UnitNamePower(UnitPrefix<S, T> prefix, UnitBaseName<S, T> baseName) : this(prefix, baseName, 1)
        {
        }

        public UnitName<S, T> UnitName { get; set; }

        public long Power { get; set; }

        public string PowerAsString => Power == 1 ? "" : Power.ToString();

        private long AbsPower => (Power >= 0) ? Power : -Power;

        public string PowerAsStringForName => AbsPower == 1 ? "" : string.Format("^{0}", AbsPower.ToString());

        public string AsString => Power == 0 ? null : string.Format("{0}{1}", UnitName.AsString, PowerAsString);

        public string AsAsciiString => Power == 0 ? null : string.Format("{0}{1}", UnitName.AsAsciiString, PowerAsString);

        public string Name => string.Format("{2}{0}{1}", UnitName.Name, PowerAsStringForName, Power < 0 ? "by " : "");
    }
}