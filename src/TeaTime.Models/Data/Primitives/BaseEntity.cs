namespace TeaTime.Models.Data.Primitives
{
    using System;

    public class BaseEntity
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
