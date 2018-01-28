namespace Unit.Lib.Core.Service
{
    public interface IUnitService<T>
    {
        DomainModel.UnitElement<T> Multiply(DomainModel.UnitElement<T> unit1, DomainModel.UnitElement<T> unit2);

        DomainModel.UnitElement<T> Divide(DomainModel.UnitElement<T> unit1, DomainModel.UnitElement<T> unit2);

        DomainModel.UnitValue<T> Add(DomainModel.UnitValue<T> unit1, DomainModel.UnitValue<T> unit2);

        DomainModel.UnitValue<T> Substract(DomainModel.UnitValue<T> unit1, DomainModel.UnitValue<T> unit2);

        DomainModel.UnitValue<T> Multiply(DomainModel.UnitValue<T> unit1, DomainModel.UnitValue<T> unit2);

        DomainModel.UnitValue<T> Divide(DomainModel.UnitValue<T> unit1, DomainModel.UnitValue<T> unit2);

        DomainModel.UnitValue<T> Parse(string value);
    }

    public interface IUnitService : IUnitService<float> { }
}