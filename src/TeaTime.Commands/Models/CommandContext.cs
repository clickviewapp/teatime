namespace TeaTime.Commands.Models
{
    using System.Collections.Generic;

    public class CommandContext
    {
        public IDictionary<string, object> Items { get; set; } = new Dictionary<string, object>();
    }
}
