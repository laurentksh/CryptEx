﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Extensions;
using CryptExApi.Models.DTO;
using CryptExApi.Models.ViewModel;
using CryptExApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptExApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IExceptionHandlerService exceptionHandler;
        private readonly IUserService userService;

        public UserController(IExceptionHandlerService exceptionHandler, IUserService userService, ILogger<UserController> logger)
        {
            this.exceptionHandler = exceptionHandler;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUser()
        {
            try {
                var userId = HttpContext.GetUserId();
                var user = await userService.GetUser(userId);

                return Ok(user);
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not get user.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("language")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeLanguage([FromQuery] string lang)
        {
            try {
                var user = await HttpContext.GetUser();
                await userService.ChangeLanguage(user, lang);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not change user's language.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("currency")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeCurrency([FromQuery] string currency)
        {
            try {
                var user = await HttpContext.GetUser();
                await userService.ChangeCurrency(user, currency);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not change user's currency.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("resetPassword")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ResetPassword(RequestPasswordChangeDTO resetPasswordDto)
        {
            try {
                var user = await HttpContext.GetUser();
                await userService.RequestPasswordChange(user, resetPasswordDto);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not request password change.");
                return exceptionHandler.Handle(ex, Request);
            }
        }

        [HttpPost("changePassword")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try {
                var user = await HttpContext.GetUser();
                await userService.ChangePassword(user, changePasswordDTO);

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not change password.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
