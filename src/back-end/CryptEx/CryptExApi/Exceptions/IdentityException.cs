using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CryptExApi.Exceptions
{

    [Serializable]
    public class IdentityException : BadRequestException
    {
        public List<IdentityError> Errors { get; set; }

        public IdentityException() : base("Could not complete operation.") { }

        public IdentityException(List<IdentityError> errors) : base(string.Join(Environment.NewLine, errors.Select(x => $"{x.Description} ({x.Code})")))
        {
            Errors = errors;
        }

        public IdentityException(string message) : base(message) { }
        public IdentityException(string message, Exception inner) : base(message, inner) { }
        protected IdentityException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
