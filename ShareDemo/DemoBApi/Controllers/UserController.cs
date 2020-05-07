using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoBApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoBApi.Controllers
{
    [ApiController]
    [Route("demobapi/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(_userService.GetUserName(Guid.NewGuid().ToString()));
        }
    }
}
