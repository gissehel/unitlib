using System.Collections.Generic;
using System.Linq;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitElement<T>
    {
        public UnitElement()
        {
        }

        public UnitElement(IEnumerable<UnitNamePower<T>> unitNamePowers)
        {
            foreach (var unitNamePower in unitNamePowers)
            {
                if (_unitNamePowers.ContainsKey(unitNamePower.UnitName.FqName))
                {
                    var power = unitNamePower.Power + _unitNamePowers[unitNamePower.UnitName.FqName].Power;
                    if (power != 0)
                    {
                        _unitNamePowers[unitNamePower.UnitName.FqName] = new UnitNamePower<T>(unitNamePower.UnitName, power);
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

        public UnitElement(UnitNamePower<T> unitNamePower) : this(new UnitNamePower<T>[] { unitNamePower })
        {
        }

        public long UnitNameCount => _unitNamePowers.Count;

        public IEnumerable<UnitNamePower<T>> GetUnitNamePowers() => OrderedUnitNamePowers.AsEnumerable();

        private Dictionary<string, UnitNamePower<T>> _unitNamePowers = new Dictionary<string, UnitNamePower<T>>();

        private IOrderedEnumerable<UnitNamePower<T>> OrderedUnitNamePowers => _unitNamePowers.Values.Where(u => u.Power != 0).OrderByDescending(u => u.Power).ThenBy(u => u.UnitName.BaseName.AsString).ThenByDescending(u => u.UnitName.Prefix.Factor * (u.UnitName.Prefix.Invert ? -1 : 1));

        public UnitDimension GetDimension() => _unitNamePowers.Values.Where(u => u.Power != 0).Select(u => (u.UnitName.BaseName.Dimension * u.Power)).Aggregate(UnitDimension.None, (s, v) => s * v);

        public string AsString => string.Join(".", OrderedUnitNamePowers.Select(u => u.AsString));

        public string AsAsciiString => string.Join(".", OrderedUnitNamePowers.Select(u => u.AsAsciiString));
    }

    public class UnitElement : UnitElement<float>
    {
        public UnitElement()
        {
        }

        public UnitElement(IEnumerable<UnitNamePower<float>> unitNamePowers) : base(unitNamePowers)
        {
        }

        public UnitElement(UnitNamePower<float> unitNamePower) : base(unitNamePower)
        {
        }
    }
}