using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CryptExApi.Models.SignalR
{
    [Authorize]
    public class DepositHub : Hub
    {
        public const string Name = "depositsdata";
    }
}
