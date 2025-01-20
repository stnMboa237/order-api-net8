using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OrderSystem.Application.Features.Order.Queries;
using OrderSystem.Application.Features.Order.Interfaces;
using OrderSystem.Domain.Entities;
using OrderSystem.Application.Features.Order.Queries.Get;
using OrderSystem.Application.Features.Order.Dtos;


namespace OrderSystem.Application.Features.Order.Handlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;


        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
             _clientRepository = clientRepository;

        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if (order == null)
            {
                return null;
            }

            var orderDto = new OrderDto
            {
                OrderId = order.Id,
                CustomerName = order.Client.Name,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            return orderDto;
        }
    }
}