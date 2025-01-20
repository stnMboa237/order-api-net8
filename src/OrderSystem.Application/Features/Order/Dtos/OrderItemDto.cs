using System;
using System.Collections.Generic;

namespace OrderSystem.Application.Features.Order.Dtos
{
    public class OrderItemDto
    {
        public string ItemName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
