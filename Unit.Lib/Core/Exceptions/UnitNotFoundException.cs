using System;
using System.Runtime.Serialization;

namespace Unit.Lib.Core.Exceptions
{
    [Serializable]
    public class UnitNotFoundException : Exception
    {
        public UnitNotFoundException()
        {
        }

        public UnitNotFoundException(string message) : base(message)
        {
        }

        public UnitNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnitNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}