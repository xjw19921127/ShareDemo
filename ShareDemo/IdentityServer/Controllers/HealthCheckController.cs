using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult IsAlive(string str) 
        {
            return new JsonResult(new { str = str, time = DateTime.Now.ToString() });
        }
    }
}
