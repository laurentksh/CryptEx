using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Models.DTO;
using CryptExApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptExApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IExceptionHandlerService exceptionHandler;

        public AuthController(ILogger<AuthController> logger, IExceptionHandlerService exceptionHandler)
        {
            this.logger = logger;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate([FromBody] AuthDTO auth)
        {
            try {

                return Ok();
            } catch (Exception ex) {
                logger.LogWarning(ex, "Could not authenticate user.");
                return exceptionHandler.Handle(ex, Request);
            }
        }
    }
}
