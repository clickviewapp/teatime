namespace TeaTime.Models
{
    using System.Collections.Generic;
    using Data.Primitives;
    
    public class Run : BaseEntity
    {
        public ulong UserId { get; set; } // ownerId
        public ulong ModuleId { get; set; }
        public string ChannelId { get; set; }
        public bool Ended { get; set; }
    }
}