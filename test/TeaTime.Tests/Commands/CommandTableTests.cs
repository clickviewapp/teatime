using System;

namespace TeaTime.Tests.Commands
{
    using TeaTime.Commands.Activation;
    using TeaTime.Commands.Attributes;
    using TeaTime.Commands.Core;
    using TeaTime.Commands.Loader;
    using TeaTime.Commands.Models;
    using Xunit;


    [CommandPrefix("test")]
    [CommandPrefix("test2")]
    class TestCommand : Command
    {
        [Command("test2")]
        public void Test(string[] args)
        {
            
        }
    }

    public class DummyActivator : ICommandActivator
    {
        public object Create(Type type)
        {
            if (type == typeof(TestCommand))
                return new TestCommand();

            throw new Exception("Unknown command type");
        }
    }

    public class CommandTableTests
    {
        private readonly ICommandActivator _activator;

        public CommandTableTests()
        {
            _activator = new DummyActivator();
        }

        [Fact]
        public void CanPopulateCommandTableWithGlobalAssemblies()
        {
            var cmdTable = new CommandTable(_activator);
            cmdTable.LoadCommands(CommandLoader.GetCommands());
        }

        [Fact]
        public void CanGetMultipleMethods()
        {
            var cmdTable = new CommandTable(_activator);
            cmdTable.LoadCommands(typeof(TestCommand));

            var testMethod = cmdTable.GetMethod("test test2");
            var testMethod2 = cmdTable.GetMethod("test2 test2");

            Assert.NotNull(testMethod);
            Assert.NotNull(testMethod2);

            Assert.Equal(testMethod.Action, testMethod2.Action);
        }
        
        [Fact]
        public void CanAddAndGetCustomCommand()
        {
            const string command = "testcmd";
            const string description = "desc";

            var cmdTable = new CommandTable(_activator);

            cmdTable.AddCommand(command, (obj, context) => new StringCommandResponse("Hello World"), description);

            var method = cmdTable.GetMethod(command);

            Assert.NotNull(method);
            Assert.NotNull(method.Action);
            Assert.Equal(command, method.Command);
            Assert.Equal(description, method.Description);
        }
    }
}
