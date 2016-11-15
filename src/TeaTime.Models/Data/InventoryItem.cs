namespace TeaTime.Models.Data
{
    using System.Collections.Generic;
    using Primitives;
    public class InventoryItem : BaseEntity
    {
        public string  ItemCode { get; set; }
        public string  Description { get; set; }
        public decimal UnitPrice { get; set; } = 0.00M;
        public int     Quantity { get; set; } = -1; // default unlimited stock = -1

        public List<InventoryItemOption> Options { get; set; } = new List<InventoryItemOption>();
    }
}
