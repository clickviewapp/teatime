namespace TeaTime.Commands.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Activation;
    using Attributes;
    using Loader;
    using Models;

    public class CommandTable
    {
        private readonly ICommandActivator _commandActivator;

        private readonly Dictionary<string, CommandMethod> _methodTable = new Dictionary<string, CommandMethod>();

        public CommandTable(ICommandActivator commandActivator)
        {
            _commandActivator = commandActivator;
        }

        public void LoadCommands(IEnumerable<Type> types)
        {
            foreach(var type in types)
                LoadCommands(type);
        }

        public void LoadCommands(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (!CommandLoader.IsValidCommand(type))
            {
                throw new Exception($"Failed to load commands for type {type}. Is not an instance of Command");
            }
            
            var methods  = typeInfo.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            var prefixes = typeInfo.GetCustomAttributes<CommandPrefixAttribute>().ToList();

            foreach (var method in methods)
            {
                foreach (var attr in method.GetCustomAttributes<CommandAttribute>())
                {
                    var action = MethodInvoker(type, method);

                    foreach (var cmdStr in GetCommandStrings(attr, prefixes))
                    {
                        if (_methodTable.ContainsKey(cmdStr))
                            throw new Exception($"Command '{cmdStr}' has already been registered");

                        _methodTable.Add(cmdStr, new CommandMethod
                        {
                            Command = cmdStr,
                            Description = attr.Description,
                            Action = action
                        });
                    }

                }
            }
        }

        private Func<object[], CommandContext, object> MethodInvoker(Type classType, MethodBase methodInfo)
        {
            return (objs, context) =>
            {
                var command = _commandActivator.Create(classType) as Command;

                if(command == null)
                    throw new Exception("Oh o");

                command.Context = context;

                return methodInfo.Invoke(command, objs);
            };
        }

        public void AddCommand(string command, Func<object[], CommandContext, ICommandResponse> action, string description = null)
        {
            _methodTable.Add(command, new CommandMethod
            {
                Command = command,
                Action = action,
                Description = description
            });
        }

        public CommandMethod GetMethod(string command)
        {
            //TODO: Make bulletproof
            if (!_methodTable.ContainsKey(command))
                return null;

            return _methodTable[command];
        }

        private static IEnumerable<string> GetCommandStrings(CommandAttribute attribute, IEnumerable<CommandPrefixAttribute> prefixes)
        {
            var cmds = new List<string>();
            
            //TODO: Cleanup
            if (!prefixes.Any())
            {
                var cmd = Parse(attribute, null);
                if (cmd != null)
                    cmds.Add(cmd.Trim());

                return cmds;
            }
            
            foreach (var prefix in prefixes)
            {
                var cmd = Parse(attribute, prefix);
                if(cmd != null)
                    cmds.Add(cmd.Trim());
            }

            return cmds;
        }

        private static string Parse(CommandAttribute attribute, CommandPrefixAttribute prefix)
        {
            var cmd = attribute?.Command;
            var pre = prefix?.Command;

            if (cmd == null && pre == null)
                return null;

            if (cmd == null)
                return pre;

            if (pre == null)
                return cmd;

            return pre + " " + cmd;
        }
    }
}
