using System.Collections.Generic;
using System.Globalization;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Exceptions;
using Unit.Lib.Core.Service;

namespace Unit.Lib.Service
{
    public class ConstantProvider : IConstantProvider
    {
        private Dictionary<string, UnitBaseName> UnitByName { get; } = new Dictionary<string, UnitBaseName>();
        private Dictionary<string, UnitBaseName> UnitBySymbol { get; } = new Dictionary<string, UnitBaseName>();
        private Dictionary<string, UnitPrefix> UnitPrefixByName { get; } = new Dictionary<string, UnitPrefix>();
        private Dictionary<string, UnitPrefix> UnitPrefixBySymbol { get; } = new Dictionary<string, UnitPrefix>();

        private void Add(UnitBaseName unitBaseName)
        {
            UnitByName[unitBaseName.Name] = unitBaseName;
            UnitBySymbol[unitBaseName.Symbol] = unitBaseName;
        }

        private void Add(UnitPrefix unitPrefix)
        {
            UnitPrefixByName[unitPrefix.Name] = unitPrefix;
            UnitPrefixBySymbol[unitPrefix.Symbol] = unitPrefix;
        }

        public ConstantProvider()
        {
            Add(new UnitPrefix { Name = "", Symbol = "", Invert = false, Factor = 1, Namespace = "" });

            Add(new UnitBaseName { Name = "metre", Symbol = "m", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "gram", Symbol = "g", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Mass });
            Add(new UnitBaseName { Name = "second", Symbol = "s", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Time });
            Add(new UnitBaseName { Name = "kelvin", Symbol = "K", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Temperature });
            Add(new UnitBaseName { Name = "ampere", Symbol = "A", Factor = 1, Namespace = "SI", Dimension = UnitDimension.ElectricCurrent });
            Add(new UnitBaseName { Name = "mole", Symbol = "mol", Factor = 6.0221415e23f, Namespace = "SI", Dimension = UnitDimension.AmountOfSubstance });
            Add(new UnitBaseName { Name = "candela", Symbol = "cd", Factor = 1, Namespace = "SI", Dimension = UnitDimension.LuminousIntensity });

            Add(new UnitPrefix { Name = "yocto", Symbol = "y", Invert = true, Factor = 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "zepto", Symbol = "z", Invert = true, Factor = 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 , Namespace = "SI" });
            Add(new UnitPrefix { Name = "atto", Symbol = "a", Invert = true, Factor = 1000f * 1000 * 1000 * 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "femto", Symbol = "f", Invert = true, Factor = 1000f * 1000 * 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "pico", Symbol = "p", Invert = true, Factor = 1000f * 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "nano", Symbol = "n", Invert = true, Factor = 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "micro", Symbol = "µ", Invert = true, Factor = 1000 * 1000 , Namespace = "SI" });
            Add(new UnitPrefix { Name = "milli", Symbol = "m", Invert = true, Factor = 1000 , Namespace = "SI" });
            Add(new UnitPrefix { Name = "centi", Symbol = "c", Invert = true, Factor = 100, Namespace = "SI" });
            Add(new UnitPrefix { Name = "deci", Symbol = "d", Invert = true, Factor = 10, Namespace = "SI" });

            Add(new UnitPrefix { Name = "deca", Symbol = "da", Invert = false, Factor = 10, Namespace = "SI" });
            Add(new UnitPrefix { Name = "hecto", Symbol = "h", Invert = false, Factor = 100, Namespace = "SI" });
            Add(new UnitPrefix { Name = "kilo", Symbol = "k", Invert = false, Factor = 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "mega", Symbol = "M", Invert = false, Factor = 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "giga", Symbol = "G", Invert = false, Factor = 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "tera", Symbol = "T", Invert = false, Factor = 1000f * 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "peta", Symbol = "P", Invert = false, Factor = 1000f * 1000 * 1000 * 1000 *1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "exa", Symbol = "E", Invert = false, Factor = 1000f * 1000 * 1000 * 1000 * 1000 * 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "zetta", Symbol = "Z", Invert = false, Factor = 1000f * 1000 * 1000 * 1000 * 1000 * 1000 *1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "yotta", Symbol = "Y", Invert = false, Factor = 1000f * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000, Namespace = "SI" });

            Add(new UnitBaseName { Name = "minute", Symbol = "min", Factor = 60, Namespace = "Time", Dimension = UnitDimension.Time });
            Add(new UnitBaseName { Name = "hour", Symbol = "h", Factor = 60 * 60, Namespace = "Time", Dimension = UnitDimension.Time });
            Add(new UnitBaseName { Name = "day", Symbol = "d", Factor = 60 * 60 * 24, Namespace = "Time", Dimension = UnitDimension.Time });
            Add(new UnitBaseName { Name = "year", Symbol = "y", Factor = 31556952, Namespace = "Time", Dimension = UnitDimension.Time }); // 31556952 = 60*60*24*365.2524
            Add(new UnitBaseName { Name = "hour", Symbol = "h", Factor = 60 * 60, Namespace = "Time", Dimension = UnitDimension.Time });

            Add(new UnitBaseName { Name = "jour", Symbol = "j", Factor = 60 * 60 * 24, Namespace = "Time_fr", Dimension = UnitDimension.Time });
            Add(new UnitBaseName { Name = "an", Symbol = "an", Factor = 31556952, Namespace = "Time_fr", Dimension = UnitDimension.Time }); // 31556952 = 60*60*24*365.2524

            Add(new UnitBaseName { Name = "Ångström", Symbol = "Å", Factor = 1e-10f, Namespace = "SI_derivative", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "litre", Symbol = "L", Factor = 1e-3f, Namespace = "SI_derivative", Dimension = UnitDimension.Length * UnitDimension.Length * UnitDimension.Length });
            Add(new UnitBaseName { Name = "dalton", Symbol = "Da", Factor = 1.660538921e-24f, Namespace = "SI_derivative", Dimension = UnitDimension.Mass });
            Add(new UnitBaseName { Name = "hertz", Symbol = "Hz", Factor = 1f, Namespace = "SI_derivative", Dimension = UnitDimension.None / UnitDimension.Time });
            Add(new UnitBaseName { Name = "newton", Symbol = "N", Factor = 1000f, Namespace = "SI_derivative", Dimension = UnitDimension.Mass * UnitDimension.Length / (UnitDimension.Time * UnitDimension.Time) });
            // Here be pascal - joule - watt - coulomb - volt - ohm - siemens - ... - rad - lumen


            Add(new UnitBaseName { Name = "inch", Symbol = "in", Factor = 2.54e-2f, Namespace = "US", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "foot", Symbol = "ft", Factor = 12 * 2.54e-2f, Namespace = "US", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "yard", Symbol = "yd", Factor = 3 * 12 * 2.54e-2f, Namespace = "US", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "furlong", Symbol = "furlong", Factor = 660 * 3 * 12 * 2.54e-2f, Namespace = "US", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "us-mile", Symbol = "mi", Factor = 5280 * 12 * 2.54e-2f, Namespace = "US", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "nautical-mile", Symbol = "nmi", Factor = 1852, Namespace = "US", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "pound-mass", Symbol = "lbm", Factor = 453.59237f, Namespace = "US", Dimension = UnitDimension.Mass });
            Add(new UnitBaseName { Name = "ounce", Symbol = "oz", Factor = 28.349523125f, Namespace = "US", Dimension = UnitDimension.Mass });

            // using AmountOfSubstance as dimension for a bit is questionnable. It's not as if using amount of substance as a dimension wasn't questionnable in the first place...
            // bit is the abreviation of bit in the IEC 60027 standard, while the abreviation is b in the IEEE 1541 standard, colliding with the abreviation of barn in the SI derivative standards.
            Add(new UnitBaseName { Name = "bit", Symbol = "bit", Factor = 1, Namespace = "IEC", Dimension = UnitDimension.AmountOfSubstance });

            // Ok, technically byte is not 8 bits, but since around 1970 meanning of byte changed from "the base of the current computer architecture" to "an octet"
            Add(new UnitBaseName { Name = "byte", Symbol = "B", Factor = 8, Namespace = "IEC", Dimension = UnitDimension.AmountOfSubstance });
            Add(new UnitBaseName { Name = "octet", Symbol = "o", Factor = 8, Namespace = "IEC", Dimension = UnitDimension.AmountOfSubstance });

            Add(new UnitPrefix { Name = "kibi", Symbol = "Ki", Invert = false, Factor = 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "mebi", Symbol = "Mi", Invert = false, Factor = 1024 * 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "gibi", Symbol = "Gi", Invert = false, Factor = 1024 * 1024 * 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "tebi", Symbol = "Ti", Invert = false, Factor = 1024f * 1024 * 1024 * 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "pebi", Symbol = "Pi", Invert = false, Factor = 1024f * 1024 * 1024 * 1024 * 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "exbi", Symbol = "Ei", Invert = false, Factor = 1024f * 1024 * 1024 * 1024 * 1024 * 1024 , Namespace = "IEC" });
            Add(new UnitPrefix { Name = "zebi", Symbol = "Zi", Invert = false, Factor = 1024f * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "yobi", Symbol = "Yi", Invert = false, Factor = 1024f * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024, Namespace = "IEC" });
        }

        public UnitPrefix GetPrefixByName(string name)
        {
            if (UnitPrefixByName.ContainsKey(name))
            {
                return UnitPrefixByName[name];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit prefix [{0}] not found.", name));
        }

        public UnitPrefix GetPrefixBySymbol(string symbol)
        {
            if (UnitPrefixBySymbol.ContainsKey(symbol))
            {
                return UnitPrefixBySymbol[symbol];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit prefix [{0}] not found.", symbol));
        }

        public UnitBaseName GetUnitByName(string name)
        {
            if (UnitByName.ContainsKey(name))
            {
                return UnitByName[name];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit [{0}] not found.", name));
        }

        public UnitBaseName GetUnitBySymbol(string symbol)
        {
            if (UnitBySymbol.ContainsKey(symbol))
            {
                return UnitBySymbol[symbol];
            }
            throw new UnitNotFoundException(string.Format(CultureInfo.InvariantCulture, "Unit [{0}] not found.", symbol));
        }
    }
}