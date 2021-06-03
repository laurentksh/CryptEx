using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptExApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptExApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly CryptExDbContext DbContext; // I know I should use a service and everything but I was too lazy to do that for such a trivial task...
        private readonly ILogger<GeneralController> Logger;

        public GeneralController(CryptExDbContext dbContext, ILogger<GeneralController> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok("OK");
        }

        [HttpGet("countries")]
        public async Task<IActionResult> Countries()
        {
            var countries = DbContext.Countries.ToList();

            return Ok(countries);
        }
    }
}
