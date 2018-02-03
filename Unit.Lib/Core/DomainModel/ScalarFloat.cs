using System;
using System.Globalization;
using Unit.Lib.Core.Exceptions;

namespace Unit.Lib.Core.DomainModel
{
    public class ScalarFloat : IScalar<float>
    {
        public ScalarFloat() : this(0f)
        {
        }

        public ScalarFloat(float value)
        {
            _value = value;
        }

        private readonly float _value;

        public float Value => _value;

        public IScalar<float> Add(IScalar<float> scalar) => new ScalarFloat(Value + scalar.Value);

        public IScalar<float> Substract(IScalar<float> scalar) => new ScalarFloat(Value - scalar.Value);

        public IScalar<float> Multiply(IScalar<float> scalar) => new ScalarFloat(Value * scalar.Value);

        public IScalar<float> Divide(IScalar<float> scalar) => new ScalarFloat(Value / scalar.Value);

        public IScalar<float> Multiply(long scalar) => new ScalarFloat(Value * scalar);

        public IScalar<float> Divide(long scalar) => new ScalarFloat(Value / scalar);

        public IScalar<float> ApplyPower(long scalar) => new ScalarFloat((float)Math.Pow(Value, scalar));

        public IScalar<float> Invert() => new ScalarFloat(1 / Value);

        public IScalar<float> Parse(string data)
        {
            try
            {
                return new ScalarFloat(float.Parse(data, CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                throw new UnitParserException(string.Format("Parsing error : Impossible to parse [{0}] as a value", data), ex);
            }
        }

        public IScalar<float> GetNeutral() => new ScalarFloat(1);

        public int CompareTo(IScalar<float> other) => Value.CompareTo(other.Value);

        public IScalar<float> GetNew(float value) => new ScalarFloat(value);

        public IScalar<float> GetNewFromFloat(float value) => new ScalarFloat(value);
    }
}