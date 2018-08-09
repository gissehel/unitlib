using Unit.Lib.Core.DomainModel;

namespace Unit.Lib.Core.Service
{
    public interface IUnitService<S, T> where S : IScalar<T>, new()
    {
        UnitValue<S, T> Add(UnitValue<S, T> unit1, UnitValue<S, T> unit2);

        UnitValue<S, T> Substract(UnitValue<S, T> unit1, UnitValue<S, T> unit2);

        UnitValue<S, T> Multiply(UnitValue<S, T> unit1, UnitValue<S, T> unit2);

        UnitValue<S, T> Divide(UnitValue<S, T> unit1, UnitValue<S, T> unit2);

        UnitValue<S, T> Parse(string value);

        UnitElement<S, T> ParseUnit(string value);

        UnitValue<S, T> Convert(UnitValue<S, T> value);

        UnitValue<S, T> Convert(UnitValue<S, T> value, UnitElement<S, T> unitElement);

        UnitValue<S, T> Convert(UnitValue<S, T> valueSource, UnitValue<S, T> valueTarget);
    }

    public interface IUnitService : IUnitService<ScalarFloat, float> { }
}