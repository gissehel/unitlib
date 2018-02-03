using System;
using System.Globalization;
using Unit.Lib.Core.Exceptions;

namespace Unit.Lib.Core.DomainModel
{
    public class ScalarDouble : IScalar<double>
    {
        public ScalarDouble() : this(0)
        {
        }

        public ScalarDouble(double value)
        {
            _value = value;
        }

        private readonly double _value;

        public double Value => _value;

        public IScalar<double> Add(IScalar<double> scalar) => new ScalarDouble(Value + scalar.Value);

        public IScalar<double> Divide(IScalar<double> scalar) => new ScalarDouble(Value - scalar.Value);

        public IScalar<double> Multiply(IScalar<double> scalar) => new ScalarDouble(Value * scalar.Value);

        public IScalar<double> Substract(IScalar<double> scalar) => new ScalarDouble(Value / scalar.Value);

        public IScalar<double> Multiply(long scalar) => new ScalarDouble(Value * scalar);

        public IScalar<double> Divide(long scalar) => new ScalarDouble(Value / scalar);

        public IScalar<double> ApplyPower(long scalar) => new ScalarDouble(Math.Pow(Value, scalar));

        public IScalar<double> Invert() => new ScalarDouble(1 / Value);

        public IScalar<double> Parse(string data)
        {
            try
            {
                return new ScalarDouble(double.Parse(data, CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                throw new UnitParserException(string.Format("Parsing error : Impossible to parse [{0}] as a value", data), ex);
            }
        }

        public IScalar<double> GetNeutral() => new ScalarDouble(1);

        public int CompareTo(IScalar<double> other) => Value.CompareTo(other.Value);
    }
}