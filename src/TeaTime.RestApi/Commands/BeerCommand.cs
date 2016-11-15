namespace TeaTime.RestApi.Commands
{
    using Models;
    using Models.SlashCommands;
    using TeaTime.Commands.Attributes;
    using TeaTime.Commands.Core;
    using TeaTime.Commands.Models;

    public class BeerCommand : Command
    {
        [Command("beer")]
        public ICommandResponse Beer(string location)
        {
            var cmd = Context.Items["SlashCommand"] as SlashCommand;

            return new SlashCommandResponse
            {
                response_type = "Ayo"
            };
        }
    }
}
