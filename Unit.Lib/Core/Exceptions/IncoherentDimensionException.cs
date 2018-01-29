using System;
using System.Runtime.Serialization;

namespace Unit.Lib.Core.Exceptions
{
    [Serializable]
    public class IncoherentDimensionException : Exception
    {
        public IncoherentDimensionException()
        {
        }

        public IncoherentDimensionException(string message) : base(message)
        {
        }

        public IncoherentDimensionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IncoherentDimensionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}