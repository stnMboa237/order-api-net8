using System;
using System.Collections.Generic;
using OrderSystem.Domain.Entities;
using MediatR;


namespace OrderSystem.Application.Features.Order.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }

        public DeleteOrderCommand(Guid orderId)
        {
            this.OrderId = orderId;
        }        

    }

}
