using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptExApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> logger;
        private readonly IExceptionHandlerService exHandler;
        private readonly IAdminService adminService;
        private readonly IUserService userService;

        public AdminController(ILogger<AdminController> logger, IExceptionHandlerService exHandler, IAdminService adminService)
        {
            this.logger = logger;
            this.exHandler = exHandler;
            this.adminService = adminService;
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromQuery] Guid? userId)
        {
            try {
                var user = await userService.GetUser(userId.Value);

                return Ok(user);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't set payment status");
                return exHandler.Handle(ex, Request);
            }
        }

        [HttpGet("deposits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDeposits([FromQuery] Guid? userId, [FromQuery] PaymentStatus? status, [FromQuery] WalletType? type)
        {
            try {
                var deposits = await adminService.GetAllDeposits(userId, status, type.GetValueOrDefault(WalletType.Fiat));

                return Ok(deposits);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't set payment status");
                return exHandler.Handle(ex, Request);
            }
        }

        [HttpPost("setPaymentStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetPaymentStatus([FromQuery] string sessionId, [FromQuery] PaymentStatus status)
        {
            try {
                await adminService.SetPaymentStatus(sessionId, status);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't set payment status");
                return exHandler.Handle(ex, Request);
            }
        }
    }
}
