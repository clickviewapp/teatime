namespace TeaTime.RestApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.SlashCommands;
    using Newtonsoft.Json;
    using TeaTime.Commands.Core;
    using TeaTime.Commands.Exceptions;
    using TeaTime.Commands.Models;
    using TeaTime.Models;
    using TeaTime.Contracts;

    [Route("teatime")]
    public class SlashCommandController : Controller
    {
        private readonly ICommandRunner _commandRunner;
        private readonly ILogger _logger;

        public SlashCommandController(ICommandRunner commandRunner, ILoggerFactory loggerFactory)
        {
            _commandRunner = commandRunner;
            _logger = loggerFactory.CreateLogger(nameof(SlashCommandController));
        }

        [HttpGet]
        public IActionResult Get(string command)
        {
            return Content("You suck");
        }

        [HttpPost]
        public async Task<IActionResult> Post(string token, string team_id, string team_domain, string channel_id, string channel_name, string user_id, string user_name, string command, string text, string response_url)
        {
            var slashCommand = new SlashCommand
            {
                token = token,
                team_domain = team_domain,
                team_id = team_id,
                channel_id = channel_id,
                channel_name = channel_name,
                user_id = user_id,
                user_name = user_name,
                command = command,
                text = text,
                response_url = response_url
            };

            try
             {
                var context  = new CommandContext();
                context.Items.Add("SlashCommand", slashCommand);

                var response = await _commandRunner.RunAsync(slashCommand.text, context);
                
                _logger.LogDebug(JsonConvert.SerializeObject(response));

                return new ObjectResult(response);
             }
             catch (CommandNotFoundException)
             {
                 //this is ok?
                 return BadRequest();
             }
             catch (Exception e)
             {
                _logger.LogError(e.Message);

                 throw;
             }
        }
    }
}
