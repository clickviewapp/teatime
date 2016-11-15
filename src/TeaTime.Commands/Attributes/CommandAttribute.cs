namespace TeaTime.Commands.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {
            
        }

        public CommandAttribute(string command)
        {
            Command = command;
        }

        public string Command { get; set; }
        public string Description { get; set; }
    }
}