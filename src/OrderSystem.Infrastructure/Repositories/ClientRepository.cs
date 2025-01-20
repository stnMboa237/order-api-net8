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
    public class ClientRepository : IClientRepository
    {
        private readonly OrderDbContext _dbContext;

        public ClientRepository(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _dbContext.Clients
                .ToListAsync();
        }

        public async Task<List<Client>> GetByNameAsync(string name)
        {
            var query = _dbContext.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            return await query.ToListAsync(); 
        }

        public async Task<Client> AddAsync(Client client)
        {
            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client?> GetByIdAsync(Guid clientId)
        {
            return await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId);
        }

        public async Task UpdateAsync(Client client)
        {
            _dbContext.Clients.Update(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid clientId)
        {
            var client = await GetByIdAsync(clientId);
            if (client != null)
            {
                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}