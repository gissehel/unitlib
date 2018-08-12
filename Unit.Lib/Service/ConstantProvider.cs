using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.DomainModel.Enumeration;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;

namespace Unit.Lib.Service
{
    public class ConstantProvider<S, T> : IConstantProvider<S, T> where S : class, IScalar<T>, new()
    {
        private S NullScalar = new S();

        protected S GetScalar(float value) => NullScalar.GetNewFromFloat(value) as S;

        public ConstantProvider()
        {
            Populate();
        }

        private Dictionary<string, UnitBaseName<S, T>> UnitByName { get; } = new Dictionary<string, UnitBaseName<S, T>>();
        private Dictionary<string, UnitBaseName<S, T>> UnitBySymbol { get; } = new Dictionary<string, UnitBaseName<S, T>>();
        private Dictionary<string, UnitPrefix<S, T>> UnitPrefixByName { get; } = new Dictionary<string, UnitPrefix<S, T>>();
        private Dictionary<string, UnitPrefix<S, T>> UnitPrefixBySymbol { get; } = new Dictionary<string, UnitPrefix<S, T>>();

        private List<UnitElement<S, T>> References { get; } = new List<UnitElement<S, T>>();
        public Dictionary<UnitBaseQuantity, UnitElement<S, T>> ReferenceByQuantity { get; } = new Dictionary<UnitBaseQuantity, UnitElement<S, T>>();

        public UnitBaseName<S, T> Add(UnitBaseName<S, T> unitBaseName)
        {
            UnitByName[unitBaseName.Name] = unitBaseName;
            UnitBySymbol[unitBaseName.Symbol] = unitBaseName;
            return unitBaseName;
        }

        protected void AddReference(UnitElement<S, T> reference)
        {
            var dimension = reference.GetDimension();
            if (dimension.QuantityCount == 1)
            {
                var quantity = UnitDimension.UnitBaseQuantities.Where(q => dimension.GetPower(q) != 0).Single();
                ReferenceByQuantity[quantity] = reference;
            }
            References.Add(reference);
        }

        protected void AddReference(UnitNamePower<S, T> reference)
        {
            AddReference(new UnitElement<S, T>(reference));
        }

        protected void AddReference(UnitPrefix<S, T> unitPrefix, UnitBaseName<S, T> unitBaseName)
        {
            AddReference(new UnitNamePower<S, T>(unitPrefix, unitBaseName));
        }

        public UnitPrefix<S, T> Add(UnitPrefix<S, T> unitPrefix)
        {
            UnitPrefixByName[unitPrefix.Name] = unitPrefix;
            UnitPrefixBySymbol[unitPrefix.Symbol] = unitPrefix;
            return unitPrefix;
        }

        public UnitPrefix<S, T> GetPrefixByName(string name)
        {
            if (UnitPrefixByName.ContainsKey(name))
            {
                return UnitPrefixByName[name];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit prefix [{0}] not found.", name));
        }

        public UnitPrefix<S, T> GetPrefixBySymbol(string symbol)
        {
            if (UnitPrefixBySymbol.ContainsKey(symbol))
            {
                return UnitPrefixBySymbol[symbol];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit prefix [{0}] not found.", symbol));
        }

        public UnitBaseName<S, T> GetUnitByName(string name)
        {
            if (UnitByName.ContainsKey(name))
            {
                return UnitByName[name];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit [{0}] not found.", name));
        }

        public UnitBaseName<S, T> GetUnitBySymbol(string symbol)
        {
            if (UnitBySymbol.ContainsKey(symbol))
            {
                return UnitBySymbol[symbol];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit [{0}] not found.", symbol));
        }

        public UnitPrefix<S, T> CreateUnitPrefix(string name, string symbol, bool invert, float factorValue, string nameSpace) => new UnitPrefix<S, T> { Name = name, Symbol = symbol, Invert = invert, Factor = GetScalar(factorValue), Namespace = nameSpace };

        public UnitPrefix<S, T> CreateUnitPrefix(string name, string symbol, bool invert, S factorScalar, string nameSpace) => new UnitPrefix<S, T> { Name = name, Symbol = symbol, Invert = invert, Factor = factorScalar, Namespace = nameSpace };

        public UnitBaseName<S, T> CreateUnitBaseName(string name, string symbol, float factorValue, string nameSpace, UnitDimension dimension) => new UnitBaseName<S, T> { Name = name, Symbol = symbol, Factor = GetScalar(factorValue), Namespace = nameSpace, Dimension = dimension };

        public UnitBaseName<S, T> CreateUnitBaseName(string name, string symbol, S factorScalar, string nameSpace, UnitDimension dimension) => new UnitBaseName<S, T> { Name = name, Symbol = symbol, Factor = factorScalar, Namespace = nameSpace, Dimension = dimension };

        public void Populate()
        {
            var noPrefix = Add(CreateUnitPrefix("", "", false, 1, ""));
            var noUnit = Add(CreateUnitBaseName("", "", 1, "", UnitDimension.None));

            var metre = Add(CreateUnitBaseName("metre", "m", 1, "SI", UnitDimension.Length));
            var gram = Add(CreateUnitBaseName("gram", "g", 1, "SI", UnitDimension.Mass));
            var second = Add(CreateUnitBaseName("second", "s", 1, "SI", UnitDimension.Time));
            var kelvin = Add(CreateUnitBaseName("kelvin", "K", 1, "SI", UnitDimension.Temperature));
            var ampere = Add(CreateUnitBaseName("ampere", "A", 1, "SI", UnitDimension.ElectricCurrent));
            var mole = Add(CreateUnitBaseName("mole", "mol", 6.0221415e23f, "SI", UnitDimension.AmountOfSubstance));
            var candela = Add(CreateUnitBaseName("candela", "cd", 1, "SI", UnitDimension.LuminousIntensity));

            Add(CreateUnitPrefix("yocto", "y", true, 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("zepto", "z", true, 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("atto", "a", true, 1000f * 1000 * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("femto", "f", true, 1000f * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("pico", "p", true, 1000f * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("nano", "n", true, 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("micro", "µ", true, 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("milli", "m", true, 1000, "SI"));
            Add(CreateUnitPrefix("centi", "c", true, 100, "SI"));
            Add(CreateUnitPrefix("deci", "d", true, 10, "SI"));

            Add(CreateUnitPrefix("deca", "da", false, 10, "SI"));
            Add(CreateUnitPrefix("hecto", "h", false, 100, "SI"));
            var kilo = Add(CreateUnitPrefix("kilo", "k", false, 1000, "SI"));
            Add(CreateUnitPrefix("mega", "M", false, 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("giga", "G", false, 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("tera", "T", false, 1000f * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("peta", "P", false, 1000f * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("exa", "E", false, 1000f * 1000 * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("zetta", "Z", false, 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000, "SI"));
            Add(CreateUnitPrefix("yotta", "Y", false, 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000, "SI"));

            Add(CreateUnitBaseName("minute", "min", 60, "Time", UnitDimension.Time));
            Add(CreateUnitBaseName("hour", "h", 60 * 60, "Time", UnitDimension.Time));
            Add(CreateUnitBaseName("day", "d", 60 * 60 * 24, "Time", UnitDimension.Time));
            Add(CreateUnitBaseName("year", "y", 31556952, "Time", UnitDimension.Time)); // 31556952 = 60*60*24*365.2524
            Add(CreateUnitBaseName("hour", "h", 60 * 60, "Time", UnitDimension.Time));

            Add(CreateUnitBaseName("jour", "j", 60 * 60 * 24, "Time_fr", UnitDimension.Time));
            Add(CreateUnitBaseName("an", "an", 31556952, "Time_fr", UnitDimension.Time)); // 31556952 = 60*60*24*365.2524

            Add(CreateUnitBaseName("Ångström", "Å", 1e-10f, "SI_derivative", UnitDimension.Length));
            Add(CreateUnitBaseName("litre", "L", 1e-3f, "SI_derivative", UnitDimension.Length * UnitDimension.Length * UnitDimension.Length));
            Add(CreateUnitBaseName("dalton", "Da", 1.660538921e-24f, "SI_derivative", UnitDimension.Mass));
            Add(CreateUnitBaseName("hertz", "Hz", 1, "SI_derivative", UnitDimension.None / UnitDimension.Time));
            Add(CreateUnitBaseName("newton", "N", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time)));
            Add(CreateUnitBaseName("pascal", "Pa", 1000, "SI_derivative", UnitDimension.Mass / (UnitDimension.Length * UnitDimension.Time * UnitDimension.Time)));
            Add(CreateUnitBaseName("joule", "J", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time)));
            Add(CreateUnitBaseName("watt", "W", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time * UnitDimension.Time)));
            Add(CreateUnitBaseName("coulomb", "C", 1, "SI_derivative", UnitDimension.Time * UnitDimension.ElectricCurrent));
            Add(CreateUnitBaseName("volt", "V", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent)));
            Add(CreateUnitBaseName("ohm", "Ω", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent * UnitDimension.ElectricCurrent)));
            Add(CreateUnitBaseName("siemens", "S", 0.0001f, "SI_derivative", UnitDimension.Time * UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent * UnitDimension.ElectricCurrent / (UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length)));
            Add(CreateUnitBaseName("farad", "F", 0.0001f, "SI_derivative", UnitDimension.Time * UnitDimension.Time * UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent * UnitDimension.ElectricCurrent / (UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length)));
            Add(CreateUnitBaseName("tesla", "T", 1000, "SI_derivative", UnitDimension.Mass / (UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent)));
            Add(CreateUnitBaseName("weber", "Wb", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent)));
            Add(CreateUnitBaseName("henry", "H", 1000, "SI_derivative", UnitDimension.Mass * UnitDimension.Length * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time * UnitDimension.ElectricCurrent * UnitDimension.ElectricCurrent)));
            Add(CreateUnitBaseName("radian", "rad", 1, "SI_derivative", UnitDimension.None));
            Add(CreateUnitBaseName("steradian", "sr", 1, "SI_derivative", UnitDimension.None));
            Add(CreateUnitBaseName("lumen", "lm", 1, "SI_derivative", UnitDimension.LuminousIntensity));

            Add(CreateUnitBaseName("inch", "in", 2.54e-2f, "US", UnitDimension.Length));
            Add(CreateUnitBaseName("foot", "ft", 12 * 2.54e-2f, "US", UnitDimension.Length));
            Add(CreateUnitBaseName("yard", "yd", 3 * 12 * 2.54e-2f, "US", UnitDimension.Length));
            Add(CreateUnitBaseName("furlong", "furlong", 660 * 3 * 12 * 2.54e-2f, "US", UnitDimension.Length));
            Add(CreateUnitBaseName("us-mile", "mi", 5280 * 12 * 2.54e-2f, "US", UnitDimension.Length));
            Add(CreateUnitBaseName("nautical-mile", "nmi", 1852, "US", UnitDimension.Length));
            Add(CreateUnitBaseName("pound-mass", "lbm", 453.59237f, "US", UnitDimension.Mass));
            Add(CreateUnitBaseName("ounce", "oz", 28.349523125f, "US", UnitDimension.Mass));

            // using AmountOfSubstance as dimension for a bit is questionnable. It's not as if using amount of substance as a dimension wasn't questionnable in the first place...
            // bit is the abreviation of bit in the IEC 60027 standard, while the abreviation is b in the IEEE 1541 standard, colliding with the abreviation of barn in the SI derivative standards.
            Add(CreateUnitBaseName("bit", "bit", 1, "IEC", UnitDimension.AmountOfSubstance));

            // Ok, technically byte is not 8 bits, but since around 1970 meanning of byte changed from "the base of the current computer architecture" to "an octet"
            Add(CreateUnitBaseName("byte", "B", 8, "IEC", UnitDimension.AmountOfSubstance));
            Add(CreateUnitBaseName("octet", "o", 8, "IEC", UnitDimension.AmountOfSubstance));

            Add(CreateUnitPrefix("kibi", "Ki", false, 1024, "IEC"));
            Add(CreateUnitPrefix("mebi", "Mi", false, 1024 * 1024, "IEC"));
            Add(CreateUnitPrefix("gibi", "Gi", false, 1024 * 1024 * 1024, "IEC"));
            Add(CreateUnitPrefix("tebi", "Ti", false, 1024f * 1024 * 1024 * 1024, "IEC"));
            Add(CreateUnitPrefix("pebi", "Pi", false, 1024f * 1024 * 1024 * 1024 * 1024, "IEC"));
            Add(CreateUnitPrefix("exbi", "Ei", false, 1024f * 1024 * 1024 * 1024 * 1024 * 1024, "IEC"));
            Add(CreateUnitPrefix("zebi", "Zi", false, 1024f * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, "IEC"));
            Add(CreateUnitPrefix("yobi", "Yi", false, 1024f * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, "IEC"));

            AddReference(noPrefix, metre);
            AddReference(kilo, gram);
            AddReference(noPrefix, second);
            AddReference(noPrefix, kelvin);
            AddReference(noPrefix, ampere);
            AddReference(noPrefix, mole);
            AddReference(noPrefix, candela);
        }
    }
}