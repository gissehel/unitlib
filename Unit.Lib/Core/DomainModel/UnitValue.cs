using System.Globalization;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitValue<S, T> where S : IScalar<T>

    {
        public UnitValue()
        {
        }

        public UnitValue(S value, UnitElement<S, T> unitElement)
        {
            Value = value;
            UnitElement = unitElement;
        }

        public UnitElement<S, T> UnitElement { get; set; }

        public S Value { get; set; }

        public string AsString => string.Format(CultureInfo.InvariantCulture, "{0}{2}{1}", Value.Value, UnitElement.AsString, UnitElement.UnitNameCount > 0 ? " " : "");

        public string AsAsciiString => string.Format(CultureInfo.InvariantCulture, "{0}{2}{1}", Value.Value, UnitElement.AsAsciiString, UnitElement.UnitNameCount > 0 ? " " : "");

        public UnitDimension GetDimension() => UnitElement.GetDimension();
    }
}