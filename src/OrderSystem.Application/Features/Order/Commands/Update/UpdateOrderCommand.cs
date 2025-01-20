using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderSystem.Application.Features.Order.Commands.Update
{
    public class UpdateOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<UpdateOrderItemCommand> Items { get; set; } = new List<UpdateOrderItemCommand>();
        
        [JsonIgnore]
        public decimal TotalAmount => CalculateTotalAmount();

        private decimal CalculateTotalAmount()
        {
            if (Items == null || Items.Count == 0)
                return 0;
                
            return Items.Sum(item => item.Quantity * item.UnitPrice);
        }
    }
}