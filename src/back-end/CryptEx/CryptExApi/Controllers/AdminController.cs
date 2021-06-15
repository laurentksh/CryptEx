using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models;
using CryptExApi.Models.Database;
using CryptExApi.Models.ViewModel;
using CryptExApi.Models.ViewModel.Admin;
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
        public async Task<IActionResult> GetUser([FromQuery] Guid userId)
        {
            try {
                var user = await userService.GetUser(userId);

                return Ok(user);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't get user.");
                return exHandler.Handle(ex, Request);
            }
        }

        [HttpGet("searchUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserViewModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchUser([FromQuery] string query)
        {
            try {
                var users = await adminService.SearchUser(query);

                return Ok(users);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't search user.");
                return exHandler.Handle(ex, Request);
            }
        }

        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatsViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStats()
        {
            try {
                var stats = await adminService.GetStats();

                return Ok(stats);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't get stats");
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
                logger.LogWarning(ex, "Couldn't get all deposits.");
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

        [HttpGet("pendingBankAccounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPendingBankAccounts()
        {
            try {
                var result = await adminService.GetPendingBankAccounts();

                return Ok(result);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't get pending bank accounts.");
                return exHandler.Handle(ex, Request);
            }
        }

        [HttpGet("setBankAccountStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetBankAccountStatus([FromQuery] Guid bankAccountId, [FromQuery] BankAccountStatus status)
        {
            try {
                await adminService.SetBankAccountStatus(bankAccountId, status);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Couldn't set bank account status");
                return exHandler.Handle(ex, Request);
            }
        }
    }
}
