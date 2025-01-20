using System;
using System.Collections.Generic;

namespace OrderSystem.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Client Client { get; set; }
        public Guid ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}