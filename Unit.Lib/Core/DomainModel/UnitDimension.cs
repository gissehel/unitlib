using System;
using System.Collections.Generic;
using System.Linq;
using Unit.Lib.Core.DomainModel.Enumeration;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitDimension
    {
        public UnitDimension()
        {
        }

        public UnitDimension(Func<UnitBaseQuantity, long> powerFactory)
        {
            foreach (var unitBaseQuantity in UnitBaseQuantities)
            {
                var power = powerFactory(unitBaseQuantity);
                if (power != 0)
                {
                    BaseValues[unitBaseQuantity] = power;
                }
            }
        }

        public UnitDimension(UnitBaseQuantity unitBaseQuantity) : this(q => unitBaseQuantity == q ? 1 : 0)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is UnitDimension)
            {
                var other = obj as UnitDimension;
                return UnitBaseQuantities.All(q => GetPower(q) == other.GetPower(q));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return UnitBaseQuantities.Aggregate(0, (i, q) => ((i * 397) ^ (q.GetHashCode() * ((int)GetPower(q)))));
        }

        public static bool operator ==(UnitDimension dim1, UnitDimension dim2)
        {
            return dim1.Equals(dim2);
        }

        public static bool operator !=(UnitDimension dim1, UnitDimension dim2)
        {
            return !(dim1.Equals(dim2));
        }

        private Dictionary<UnitBaseQuantity, long> BaseValues { get; } = new Dictionary<UnitBaseQuantity, long>();

        public long GetPower(UnitBaseQuantity unitBaseQuantity) => BaseValues.TryGetValue(unitBaseQuantity, out long power) ? power : 0;

        public bool HasQuantity(UnitBaseQuantity unitBaseQuantity) => BaseValues.ContainsKey(unitBaseQuantity) && BaseValues[unitBaseQuantity] != 0;

        public long QuantityCount => BaseValues.Count;

        public string AsString => string.Join(".", UnitBaseQuantities.Select(u => new Tuple<UnitBaseQuantity, long>(u, GetPower(u))).Where(t => t.Item2 != 0).Select(t => string.Format("{0}{1}", DimensionNames[t.Item1], t.Item2 == 1 ? "" : t.Item2.ToString())));

        public static List<UnitBaseQuantity> UnitBaseQuantities = new List<UnitBaseQuantity> { UnitBaseQuantity.Length, UnitBaseQuantity.Mass, UnitBaseQuantity.Time, UnitBaseQuantity.ElectricCurrent, UnitBaseQuantity.Temperature, UnitBaseQuantity.AmountOfSubstance, UnitBaseQuantity.LuminousIntensity };

        public static UnitDimension operator *(UnitDimension value1, UnitDimension value2) => new UnitDimension(unitBaseQuantity => value1.GetPower(unitBaseQuantity) + value2.GetPower(unitBaseQuantity));

        public static UnitDimension operator *(UnitDimension value1, long power) => new UnitDimension(unitBaseQuantity => value1.GetPower(unitBaseQuantity) * power);

        public static UnitDimension operator /(UnitDimension value1, UnitDimension value2) => new UnitDimension(unitBaseQuantity => value1.GetPower(unitBaseQuantity) - value2.GetPower(unitBaseQuantity));

        public static UnitDimension operator -(UnitDimension value) => new UnitDimension(unitBaseQuantity => -value.GetPower(unitBaseQuantity));

        private static readonly Dictionary<UnitBaseQuantity, string> DimensionNames = new Dictionary<UnitBaseQuantity, string>
        {
            { UnitBaseQuantity.Length, "L"},
            { UnitBaseQuantity.Mass, "M"},
            { UnitBaseQuantity.Time, "T"},
            { UnitBaseQuantity.ElectricCurrent, "I"},
            { UnitBaseQuantity.Temperature, "Θ"},
            { UnitBaseQuantity.AmountOfSubstance, "N"},
            { UnitBaseQuantity.LuminousIntensity, "J"},
        };

        public static UnitDimension None = new UnitDimension();
        public static UnitDimension Length = new UnitDimension(UnitBaseQuantity.Length);
        public static UnitDimension Mass = new UnitDimension(UnitBaseQuantity.Mass);
        public static UnitDimension Time = new UnitDimension(UnitBaseQuantity.Time);
        public static UnitDimension ElectricCurrent = new UnitDimension(UnitBaseQuantity.ElectricCurrent);
        public static UnitDimension Temperature = new UnitDimension(UnitBaseQuantity.Temperature);
        public static UnitDimension LuminousIntensity = new UnitDimension(UnitBaseQuantity.LuminousIntensity);
        public static UnitDimension AmountOfSubstance = new UnitDimension(UnitBaseQuantity.AmountOfSubstance);
    }
}