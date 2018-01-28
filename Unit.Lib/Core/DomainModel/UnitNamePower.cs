namespace Unit.Lib.Core.DomainModel
{
    public class UnitNamePower<T>
    {
        public UnitNamePower(UnitName<T> unitName, long power)
        {
            UnitName = unitName;
            Power = power;
        }

        public UnitNamePower(UnitName<T> unitName) : this(unitName, 1)
        {
        }

        public UnitName<T> UnitName { get; set; }

        public long Power { get; set; }

        public string PowerAsString => Power == 1 ? "" : Power.ToString();

        public string AsString => Power == 0 ? null : string.Format("{0}{1}", UnitName.AsString, PowerAsString);

        public string AsAsciiString => Power == 0 ? null : string.Format("{0}{1}", UnitName.AsAsciiString, PowerAsString);
    }

    public class UnitNamePower : UnitNamePower<float>
    {
        public UnitNamePower(UnitName<float> unitName) : base(unitName)
        {
        }

        public UnitNamePower(UnitName<float> unitName, long power) : base(unitName, power)
        {
        }
    }
}