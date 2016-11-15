namespace TeaTime.RestApi.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Models;
    using Models.SlashCommands;
    using TeaTime.Commands.Attributes;
    using TeaTime.Commands.Core;
    using TeaTime.Commands.Models;

    [CommandPrefix("tea")]
    public class TeaCommand : Command
    {
        private readonly IRunService _teaTimeService;
        private readonly IUserService _userService;

        public TeaCommand(IRunService teaTimeService, IUserService userService)
        {
            _teaTimeService = teaTimeService;
            _userService = userService;
        }

        [Command]
        public async Task<ICommandResponse> Tea()
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

            var success = await _teaTimeService.Start(cmd);

            if (!success)
                return new SlashCommandResponse("Cannot start a round of tea, there is already a round in progress.", true);

            return new SlashCommandResponse($"{cmd.user_name} wants :Tea:, lets start a tea round. Type `/teatime tea join <orderType>` to opt in for this round of tea.");
        }

        [Command("join")]
        public async Task<ICommandResponse> Order(string tea = "")
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

            var teaTime = await _teaTimeService.Get(cmd.channel_id);

            if(teaTime == null) 
                return new SlashCommandResponse("There is no round of tea in progress, type `/teatime tea` to start a round.", true);

            var order = await _teaTimeService.AddOrder(cmd, tea);
            if (order == null)
                return new SlashCommandResponse("Whoops failed to add your order", true);
            
            var user = await _userService.Get(order.UserId);

            return new SlashCommandResponse($"{user.Name} has joined this round of tea!", false);
        }

        [Command("end")]
        public async Task<ICommandResponse> End()
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

            var teaTime = await _teaTimeService.Get(cmd.channel_id);

            if(teaTime == null || teaTime.Ended)
                return new SlashCommandResponse("There is no round of tea in progress, type `/teatime tea` to start a round.", true);

            var orders = (await _teaTimeService.GetOrders(teaTime)).ToList();

            if(orders == null || !orders.Any())
                return new SlashCommandResponse("No one has joined this round yet :(", true);

            var randomNumber = new Random().Next(orders.Count - 1);
            var brewologist = orders.ElementAt(randomNumber);

            if(!await _teaTimeService.End(cmd.channel_id))
                return new SlashCommandResponse("Whoops. Cant end this round for some reason", true);

            return new SlashCommandResponse( $"Congratulations @{brewologist.User.Name}, you drew the short straw and are making the :tea:. Here's the Order:\n {orders.Select(e => $@"{e.User.Name}: {e.Text}").Aggregate((memo, a) => memo + "\n" + a)}");
        }
    }
}