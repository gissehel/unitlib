using System.Globalization;

namespace Unit.Lib.Core.DomainModel
{
    public class UnitName<T>
    {
        public UnitBaseName<T> BaseName { get; set; }

        public UnitPrefix Prefix { get; set; }

        public string FqName => string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Prefix.FqName, BaseName.FqName);

        public string AsString => string.Format(CultureInfo.InvariantCulture, "{0}{1}", Prefix.AsString, BaseName.AsString);

        public string AsAsciiString => string.Format(CultureInfo.InvariantCulture, "{0}{1}", Prefix.AsAsciiString, BaseName.AsAsciiString);
    }

    public class UnitName : UnitName<float> { }
}