using System;
using System.Collections.Generic;

namespace OrderSystem.Application.Features.Order.Dtos
{
    public class OrderDto 
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal TotalAmount { get; set; }

    }

}
