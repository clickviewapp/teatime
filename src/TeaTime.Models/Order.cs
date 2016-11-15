namespace TeaTime.Models
{
    using System;

    public class Order
    {
        public ulong Id { get; set; }
        public string Text { get; set; }
        public ulong UserId { get; set; }
        public ulong RunId { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class OrderViewModel : Order
    {
        public OrderViewModel(Order order)
        {
            Id = order.Id;
            Text = order.Text;
            UserId = order.UserId;
            RunId = order.RunId;
            DateCreated = order.DateCreated;
        }

        public User User { get; set; }   
    }
}