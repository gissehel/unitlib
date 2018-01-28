using System;

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
    }
}