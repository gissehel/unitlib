using System;

namespace Unit.Lib.Core.DomainModel
{
    public interface IScalar<T> : IComparable<IScalar<T>>
    {
        T Value { get; }

        IScalar<T> Add(IScalar<T> scalar);

        IScalar<T> Substract(IScalar<T> scalar);

        IScalar<T> Multiply(IScalar<T> scalar);

        IScalar<T> Divide(IScalar<T> scalar);

        IScalar<T> Multiply(long scalar);

        IScalar<T> Divide(long scalar);

        IScalar<T> ApplyPower(long scalar);

        IScalar<T> Invert();

        IScalar<T> Parse(string data);

        IScalar<T> GetNeutral();
    }
}