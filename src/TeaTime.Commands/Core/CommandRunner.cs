namespace TeaTime.Commands.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Models;

    public interface ICommandRunner
    {
        Task<ICommandResponse> RunAsync(string command, CommandContext context);
    }

    public class CommandRunner : ICommandRunner
    {
        private readonly CommandTable _commandTable;

        public CommandRunner(CommandTable commandTable)
        {
            _commandTable = commandTable;
        }

        public Task<ICommandResponse> RunAsync(string command, CommandContext context)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));

            string[] args;
            var method = GetMethod(command, out args);

            if (method == null)
                throw new CommandNotFoundException(command);

            try
            {
                var result = method.Action(args, context);

                var task = result as Task<ICommandResponse>;
                if (task != null)
                    return task;

                var response = result as ICommandResponse;
                if (response != null)
                    return Task.FromResult(response);
            }
            catch
            {
                //fine
            }

            throw new Exception("Failed to run command");
        }

        private CommandMethod GetMethod(string message, out string[] args)
        {
            var tokens = Tokenizer.Tokenizer.Tokenize(message).ToArray();
            var index = tokens.Length;

            while (index != 0)
            {
                var method = _commandTable.GetMethod(string.Join(" ", tokens.Take(index)));

                if (method != null)
                {
                    args = tokens.Skip(index).ToArray();
                    return method;
                }

                --index;
            }

            args = null;
            return null;
        }
    }
}
