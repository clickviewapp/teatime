namespace TeaTime.Commands
{
    using System;
    using Models;

    public class CommandMethod
    {
        public Func<object[], CommandContext, object> Action { get; set; }
        public string Command { get; set; }
        public string Description { get; set; }
    }
}
