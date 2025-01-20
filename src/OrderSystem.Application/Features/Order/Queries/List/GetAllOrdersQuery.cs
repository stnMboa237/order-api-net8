using System;
using MediatR;
using OrderSystem.Application.Features.Order.Dtos;

namespace OrderSystem.Application.Features.Order.Queries.List
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
