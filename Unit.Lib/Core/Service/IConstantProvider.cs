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
    }
}