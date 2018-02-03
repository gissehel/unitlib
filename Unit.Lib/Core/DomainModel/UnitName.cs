using System.Globalization;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitName<S, T> where S : IScalar<T>
    {
        public UnitName(UnitPrefix<S, T> prefix, UnitBaseName<S, T> baseName)
        {
            Prefix = prefix;
            BaseName = baseName;
        }

        public UnitBaseName<S, T> BaseName { get; set; }

        public UnitPrefix<S, T> Prefix { get; set; }

        public string FqName => string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Prefix.FqName, BaseName.FqName);

        public string Name => string.Format(CultureInfo.InvariantCulture, "{0}{1}", Prefix.Name, BaseName.Name);

        public string AsString => string.Format(CultureInfo.InvariantCulture, "{0}{1}", Prefix.AsString, BaseName.AsString);

        public string AsAsciiString => string.Format(CultureInfo.InvariantCulture, "{0}{1}", Prefix.AsAsciiString, BaseName.AsAsciiString);
    }
}