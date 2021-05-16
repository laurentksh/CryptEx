using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptExApi.Models.Database
{
    public class FiatDeposit
    {
        public Guid Id { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string StripeSessionId { get; set; }
    }

    public enum PaymentStatus
    {
        Failed,
        Success,
        Pending
    }
}
