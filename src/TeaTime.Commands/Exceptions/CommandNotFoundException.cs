namespace TeaTime.Commands.Exceptions
{
    using System;

    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException(string commandName) : base($"Command {commandName} could not be found")
        {
            
        }
    }
}
