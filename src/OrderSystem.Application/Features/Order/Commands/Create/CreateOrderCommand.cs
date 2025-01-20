using System;
using System.Collections.Generic;
using OrderSystem.Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;


namespace OrderSystem.Application.Features.Order.Commands.Create
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid OrderId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<CreateOrderItemCommand> Items { get; set; } = new List<CreateOrderItemCommand>();
        
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
