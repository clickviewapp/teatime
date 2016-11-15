namespace TeaTime.Models.Data
{
    using Primitives;

    public class Module : BaseEntity
    {
        public string Command { get; set; }
        public string Description { get; set; }
    }
}