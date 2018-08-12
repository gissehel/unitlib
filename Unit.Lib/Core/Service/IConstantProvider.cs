using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.DomainModel.Enumeration;

namespace Unit.Lib.Core.Service
{
    public interface IConstantProvider<S, T> where S : IScalar<T>
    {
        Dictionary<UnitBaseQuantity, UnitElement<S, T>> ReferenceByQuantity { get; }

        UnitBaseName<S, T> GetUnitByName(string name);

        UnitBaseName<S, T> GetUnitBySymbol(string symbol);

        UnitPrefix<S, T> GetPrefixByName(string name);

        UnitPrefix<S, T> GetPrefixBySymbol(string symbol);

        UnitBaseName<S, T> CreateUnitBaseName(string name, string symbol, S factorScalar, string nameSpace, UnitDimension dimension);

        UnitPrefix<S, T> CreateUnitPrefix(string name, string symbol, bool invert, S factorScalar, string nameSpace);

        UnitBaseName<S, T> Add(UnitBaseName<S, T> unitBaseName);

        UnitPrefix<S, T> Add(UnitPrefix<S, T> unitPrefix);
    }
}