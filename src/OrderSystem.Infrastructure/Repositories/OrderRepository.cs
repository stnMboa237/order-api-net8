using Microsoft.EntityFrameworkCore;
using OrderSystem.Application.Features.Order.Interfaces;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderSystem.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _dbContext;

        public OrderRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllAsync(int? page, int? pageSize)
        {
            var query = _dbContext.Orders.Include(o => o.Items) 
                .Include(o => o.Client).AsQueryable();
            
            if (page.HasValue) {
                query = query.Skip(page.Value - 1);
            }
            if (pageSize.HasValue) {
                query = query.Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync(Guid clientId, DateTime? startDate, DateTime? endDate, int? page, int? pageSize)
        {
            var query = _dbContext.Orders.Include(o => o.Items).Include(o => o.Client).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date <= endDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(clientId.ToString()))
            {
                query = query.Where(o => o.ClientId == clientId);
            }

            if (page.HasValue) {
                query = query.Skip(page.Value - 1);
            }
            if (pageSize.HasValue) {
                query = query.Take(pageSize.Value);
            }   

            return await query.ToListAsync(); 
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid orderId)
        {
            var order = await GetByIdAsync(orderId);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}