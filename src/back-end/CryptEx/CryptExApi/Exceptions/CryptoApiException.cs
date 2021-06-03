using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Exceptions
{
    [Serializable]
    public class CryptoApiException : Exception
    {
        public Coinbase.Models.Error[] Errors { get; set; }

        public CryptoApiException() : base() { }
        public CryptoApiException(string message) : base(message) { }
        public CryptoApiException(string message, Exception inner) : base(message, inner) { }
        public CryptoApiException(Coinbase.Models.Error[] errors) : base() { Errors = errors; }
        public CryptoApiException(string message, Coinbase.Models.Error[] errors) : base(message) { Errors = errors; }
        protected CryptoApiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
