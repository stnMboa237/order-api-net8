using System;
using MediatR;
using OrderSystem.Application.Features.Order.Dtos;

namespace OrderSystem.Application.Features.Order.Queries.Get
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }

        public GetOrderByIdQuery(Guid orderId)
        {
            this.OrderId = orderId;
        }

    }
}
