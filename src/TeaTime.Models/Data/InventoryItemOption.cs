namespace TeaTime.Models.Data
{
    using Primitives;

    public class InventoryItemOption : BaseEntity
    {
        public string Value { get; set; }

        public decimal Cost { get; set; } = 0.00M;

        public ulong InventoryItemId { get; set; }
    }
}