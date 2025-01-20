using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderEntity = OrderSystem.Domain.Entities.Order;

namespace OrderSystem.Application.Features.Order.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderEntity> AddAsync(OrderEntity order); 
        Task<OrderEntity?> GetByIdAsync(Guid orderId); 
        Task<List<OrderEntity>> GetAllAsync(int? page, int? pageSize);
        Task<List<OrderEntity>> GetAllAsync(Guid clientId, DateTime? startDate, DateTime? endDate, int? page, int? pageSize);
        Task UpdateAsync(OrderEntity order);
        Task DeleteAsync(Guid orderId); 
    }
}