using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.Features.Order.Commands.Create;
using OrderSystem.Application.Features.Order.Commands.Delete;
using OrderSystem.Application.Features.Order.Commands.Update;
using OrderSystem.Application.Features.Order.Queries.Get;
using OrderSystem.Application.Features.Order.Queries.List;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using OrderSystem.Application.Features.Order.Dtos;

namespace OrderSystem.Api.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public OrdersController(IMediator mediator, IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
        {
            _mediator = mediator;
            _cache = cache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand)
        {
            var orderId = await _mediator.Send(createOrderCommand);
            
            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = orderId },
                orderId 
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] DateTime? startDate,[FromQuery] DateTime? endDate, [FromQuery] string? clientName, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllOrdersQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                ClientName = clientName ?? string.Empty,
                Page = page,
                PageSize = pageSize
            };

            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var cachedOrder = await _cache.GetStringAsync($"order:{id}");
            if (!string.IsNullOrEmpty(cachedOrder))
            {
                var orderDtoCached = JsonConvert.DeserializeObject<OrderDto>(cachedOrder);
                return Ok(orderDtoCached);
            }

            var query = new GetOrderByIdQuery(id);
            var orderDto = await _mediator.Send(query);

            if (orderDto == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            var serializedOrder = JsonConvert.SerializeObject(orderDto);
            await _cache.SetStringAsync($"order:{id}", serializedOrder, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache expira após 30 minutos
            });

            return Ok(orderDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderCommand order)
        {
            var result = await _mediator.Send(order);

            if (!result)
            {
                return NotFound(new { message = "Order not found." });
            }

            await _cache.RemoveAsync($"order:{id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(new { message = "Order not found." });
            }

            await _cache.RemoveAsync($"order:{id}");
            return NoContent();
        }

    }
}