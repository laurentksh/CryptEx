using System;

namespace CryptExApi.Exceptions
{
    [Serializable]
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Insufficient funds.") { }
        public InsufficientFundsException(string message) : base(message) { }
        public InsufficientFundsException(string message, Exception inner) : base(message, inner) { }
        protected InsufficientFundsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
