using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientEntity = OrderSystem.Domain.Entities.Client;

namespace OrderSystem.Application.Features.Order.Interfaces
{
    public interface IClientRepository
    {
        Task<ClientEntity> AddAsync(ClientEntity client); 
        Task<ClientEntity?> GetByIdAsync(Guid clientId); 
        Task<List<ClientEntity>> GetAllAsync();
        Task<List<ClientEntity>> GetByNameAsync(string clientName);
        Task UpdateAsync(ClientEntity client);
        Task DeleteAsync(Guid clientId); 
    }
}