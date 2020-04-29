using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContainerApp.Controllers
{
    public class HealthCheckController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetClaims()
        {
            var data = from c in User.Claims select new { c.Type, c.Value };
            var dict = new Dictionary<string, string>();
            foreach (var x in data.ToList())
            {
                dict[x.Type] = x.Value;
            }
            return new JsonResult(dict);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult IsAlive(string str) 
        {
            return new JsonResult(new { str = str, time = DateTime.Now.ToString() });
        }
    }
}
