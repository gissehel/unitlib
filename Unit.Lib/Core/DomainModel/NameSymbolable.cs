namespace Unit.Lib.Core.DomainModel
{
    public class NameSymbolable : Symbolable
    {
        public string Name { get; set; }

        private string _namespace = null;

        public string Namespace
        {
            get => _namespace ?? "None";
            set => _namespace = value;
        }

        private string _fqName = null;

        public string FqName
        {
            get => _fqName ?? Namespace + "/" + Name;
            set => _fqName = value;
        }
    }
}