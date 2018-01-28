namespace Unit.Lib.Core.Service
{
    public interface IConstantProvider
    {
        DomainModel.UnitBaseName GetUnitByName(string name);

        DomainModel.UnitBaseName GetUnitBySymbol(string symbol);

        DomainModel.UnitPrefix GetPrefixByName(string name);

        DomainModel.UnitPrefix GetPrefixBySymbol(string symbol);
    }
}