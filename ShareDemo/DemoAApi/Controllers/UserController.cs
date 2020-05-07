using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DemoAApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DemoAApi.Controllers
{
    [ApiController]
    [Route("demoaapi/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IComponentContext componentContext)
        {
            _logger = logger;
            _userService = componentContext.Resolve<IUserService>();
        }

        [HttpGet]
        [Route("GetUserName")]
        public ActionResult<string> Get()
        {
            return Ok(_userService.GetUserName(Guid.NewGuid().ToString()));
        }

        [HttpGet]
        [Route("GetUserNameByMediatR")]
        public ActionResult<string> GetUserNameByMediatR()
        {
            return Ok(_userService.GetUserNameByMediatRAsync(Guid.NewGuid().ToString()).Result);
        }
    }
}
