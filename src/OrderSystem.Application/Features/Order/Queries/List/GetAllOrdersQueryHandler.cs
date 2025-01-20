using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderSystem.Application.Features.Order.Interfaces;
using OrderSystem.Application.Features.Order.Dtos;

namespace OrderSystem.Application.Features.Order.Queries.List
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            List<OrderSystem.Domain.Entities.Order> orders;

            var client = await _clientRepository.GetByNameAsync(request.ClientName);
            if (client == null || client.Count == 0)
            {
                throw new ArgumentException($"Client with name '{request.ClientName}' does not exist. Please add the client.");
            }

            
            if (request.StartDate.HasValue || request.EndDate.HasValue || !string.IsNullOrEmpty(request.ClientName))
            {
                orders = await _orderRepository.GetAllAsync(client[0].Id, request.StartDate, request.EndDate, request.Page, request.PageSize);
            }
            else
            {
                orders = await _orderRepository.GetAllAsync(request.Page, request.PageSize);
            }

            var orderDtos = orders.Select(order => new OrderDto
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
            }).ToList();

            return orderDtos;
        }
    }
}