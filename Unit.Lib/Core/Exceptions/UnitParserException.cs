using System;
using System.Runtime.Serialization;

namespace Unit.Lib.Core.Exceptions
{
    [Serializable]
    public class UnitParserException : Exception
    {
        public UnitParserException()
        {
        }

        public UnitParserException(string message) : base(message)
        {
        }

        public UnitParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnitParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}