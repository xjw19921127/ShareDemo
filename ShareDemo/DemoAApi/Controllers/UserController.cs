using Autofac;
using ContainerApp.Event;
using DemoAApi.Handlers;
using DemoAApi.Services;
using EventBus.Absratctions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DemoAApi.Controllers
{
    [ApiController]
    [Route("demoaapi/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IEventBus eventBus;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IComponentContext componentContext, IEventBus eventBus)
        {
            _logger = logger;
            this.eventBus = eventBus;
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

        [HttpGet]
        [Route("TestMQPublish")]
        public ActionResult<string> TestMQPublish()
        {
            var eventMessage = new TestIntegrationEvent(Guid.NewGuid().ToString());
            eventBus.Publish(eventMessage);
            return Ok("");
        }

        [HttpGet]
        [Route("TestMQUnBind")]
        public ActionResult<string> TestMQUnBind() 
        {
            eventBus.UnSubscribe<TestIntegrationEvent, TestEventHandler>();
            eventBus.UnSubscribe<TestIntegrationEvent, TestEventHandler2>();
            return Ok("");
        }

        [HttpGet]
        [Route("TestMQBind")]
        public ActionResult<string> TestMQBind()
        {
            eventBus.Subscribe<TestIntegrationEvent, TestEventHandler>();
            eventBus.Subscribe<TestIntegrationEvent, TestEventHandler2>();
            return Ok("");
        }
    }
}
