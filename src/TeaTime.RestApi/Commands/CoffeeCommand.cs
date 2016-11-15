namespace TeaTime.RestApi.Commands
{
    using Contracts;
    using Models;
    using Models.SlashCommands;
    using System.Linq;
    using Contracts.Data.Repositories;
    using Contracts.Data.Repository;
    using Newtonsoft.Json;
    using TeaTime.Commands.Attributes;
    using TeaTime.Commands.Core;
    using TeaTime.Commands.Models;
    
    [CommandPrefix("coffee")]
    public class CoffeeCommand : Command
    {
        private readonly IRunService _runService;
        private readonly IModuleRepository _repository;

        public CoffeeCommand(IRunService runService, IModuleRepository repository)
        {
            _runService = runService;
            _repository = repository;
        }

        [Command("")]
        public ICommandResponse Coffee(string location)
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

            _runService.Start(cmd);
            
            var items = _repository.GetInventory("coffee").Result;

            return new SlashCommandResponse()
            {
                response_type = "in_channel",
                Text = "Hello"
            };
        }

        [Command("order")]
        public ICommandResponse Order(string coffee)
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

//            _runService.AddOrder(cmd);

            return new SlashCommandResponse
            {
                response_type = "in_channel",
                Text = "Hello World"
            };
        }

        [Command("list")]
        public ICommandResponse List()
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

            var teaTime = _runService.Get(cmd.channel_id).Result;

            return new SlashCommandResponse()
            {
                response_type = "in_channel",
                //Text = teaTime.Orders.Select(x => x.Type).Aggregate("", (memo, s1) => memo + " " + s1)
            };
        }
    }
}