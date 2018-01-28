using System.Globalization;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitValue<T>
    {
        public UnitValue()
        {
        }

        public UnitValue(T value, UnitElement<T> unitElement)
        {
            Value = value;
            UnitElement = unitElement;
        }

        public UnitElement<T> UnitElement { get; set; }

        public T Value { get; set; }

        public string AsString => string.Format(CultureInfo.InvariantCulture, "{0}{2}{1}", Value, UnitElement.AsString, UnitElement.UnitNameCount > 0 ? " " : "");

        public string AsAsciiString => string.Format(CultureInfo.InvariantCulture, "{0}{2}{1}", Value, UnitElement.AsAsciiString, UnitElement.UnitNameCount > 0 ? " " : "");

        public UnitDimension GetDimension() => UnitElement.GetDimension();
    }

    public class UnitValue : UnitValue<float>
    {
        public UnitValue()
        {
        }

        public UnitValue(float value, UnitElement<float> unitElement) : base(value, unitElement)
        {
        }
    }
}