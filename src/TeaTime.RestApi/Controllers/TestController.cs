using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeaTime.RestApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.SlashCommands;

    public class TestController : Controller
    {
        [Route("test"), HttpGet]
        public IActionResult Test()
        {
            var cmd = new SlashCommandResponse("Hello\nworld");

            return new ObjectResult(cmd);
        }
    }
}
