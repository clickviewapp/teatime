namespace TeaTime.Commands.Core
{
    using Models;

    public abstract class Command
    {
        public CommandContext Context { get; internal set; }
    }
}
