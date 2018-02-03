using System.Collections.Generic;
using System.Linq;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitElement<S, T> where S : IScalar<T>
    {
        public UnitElement()
        {
        }

        public UnitElement(IEnumerable<UnitNamePower<S, T>> unitNamePowers)
        {
            foreach (var unitNamePower in unitNamePowers)
            {
                if (_unitNamePowers.ContainsKey(unitNamePower.UnitName.FqName))
                {
                    var power = unitNamePower.Power + _unitNamePowers[unitNamePower.UnitName.FqName].Power;
                    if (power != 0)
                    {
                        _unitNamePowers[unitNamePower.UnitName.FqName] = new UnitNamePower<S, T>(unitNamePower.UnitName, power);
                    }
                    else
                    {
                        _unitNamePowers.Remove(unitNamePower.UnitName.FqName);
                    }
                }
                else if (unitNamePower.Power != 0)
                {
                    _unitNamePowers[unitNamePower.UnitName.FqName] = unitNamePower;
                }
            }
        }

        public UnitElement(UnitNamePower<S, T> unitNamePower) : this(new UnitNamePower<S, T>[] { unitNamePower })
        {
        }

        public long UnitNameCount => _unitNamePowers.Count;

        public IEnumerable<UnitNamePower<S, T>> GetUnitNamePowers() => OrderedUnitNamePowers.AsEnumerable();

        private Dictionary<string, UnitNamePower<S, T>> _unitNamePowers = new Dictionary<string, UnitNamePower<S, T>>();

        private IOrderedEnumerable<UnitNamePower<S, T>> OrderedUnitNamePowers => _unitNamePowers.Values.Where(u => u.Power != 0).OrderByDescending(u => u.Power).ThenBy(u => u.UnitName.BaseName.AsString).ThenByDescending(u => u.UnitName.Prefix.Factor.Multiply(u.UnitName.Prefix.Invert ? -1 : 1));

        public UnitDimension GetDimension() => _unitNamePowers.Values.Where(u => u.Power != 0).Select(u => (u.UnitName.BaseName.Dimension * u.Power)).Aggregate(UnitDimension.None, (s, v) => s * v);

        public string AsString => string.Join(".", OrderedUnitNamePowers.Select(u => u.AsString));

        public string AsAsciiString => string.Join(".", OrderedUnitNamePowers.Select(u => u.AsAsciiString));

        public string Name => string.Join(" ", OrderedUnitNamePowers.Select(u => u.Name));

        public void Simplify()
        {
            if (_unitNamePowers.Select(unp => unp.Value.UnitName.BaseName.Dimension.QuantityCount > 0).Any())
            {
                if (_unitNamePowers.ContainsKey("/:/"))
                {
                    _unitNamePowers.Remove("/:/");
                }
            }
        }
    }
}