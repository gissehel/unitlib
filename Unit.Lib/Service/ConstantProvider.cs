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
            Add(new UnitBaseName { Name = "metre", Symbol = "m", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Length });
            Add(new UnitBaseName { Name = "gram", Symbol = "g", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Mass });
            Add(new UnitBaseName { Name = "second", Symbol = "s", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Time });
            Add(new UnitBaseName { Name = "kelvin", Symbol = "K", Factor = 1, Namespace = "SI", Dimension = UnitDimension.Temperature });
            Add(new UnitBaseName { Name = "ampere", Symbol = "A", Factor = 1, Namespace = "SI", Dimension = UnitDimension.ElectricCurrent });
            Add(new UnitBaseName { Name = "mole", Symbol = "mol", Factor = 1, Namespace = "SI", Dimension = UnitDimension.AmountOfSubstance });
            Add(new UnitBaseName { Name = "candela", Symbol = "cd", Factor = 1, Namespace = "SI", Dimension = UnitDimension.LuminousIntensity });

            Add(new UnitPrefix { Name = "", Symbol = "", Invert = false, Factor = 1, Namespace = "" });
            Add(new UnitPrefix { Name = "kilo", Symbol = "k", Invert = false, Factor = 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "milli", Symbol = "m", Invert = true, Factor = 1000, Namespace = "SI" });
            Add(new UnitPrefix { Name = "mega", Symbol = "M", Invert = false, Factor = 1000000, Namespace = "SI" });

            Add(new UnitPrefix { Name = "kibi", Symbol = "Ki", Invert = false, Factor = 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "mebi", Symbol = "Mi", Invert = false, Factor = 1024 * 1024, Namespace = "IEC" });
            Add(new UnitPrefix { Name = "gibi", Symbol = "Gi", Invert = false, Factor = 1024 * 1024, Namespace = "IEC" });
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