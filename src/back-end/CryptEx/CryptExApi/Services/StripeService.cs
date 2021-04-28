using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace CryptExApi.Services
{
    public interface IStripeService
    {
        Task CreateDeposit(Session session);

        Task FullfillDeposit(Session session);

        Task SetDepositAsFailed(Session session);
    }

    public class StripeService : IStripeService
    {
        public Task CreateDeposit(Session session)
        {
            throw new NotImplementedException();
        }

        public Task FullfillDeposit(Session session)
        {
            throw new NotImplementedException();
        }

        public Task SetDepositAsFailed(Session session)
        {
            throw new NotImplementedException();
        }
    }
}
