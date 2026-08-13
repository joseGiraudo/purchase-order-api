using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Domain.Entities;

namespace PurchaseOrders.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task<User?> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeactivateAsync(User user);
    }
}
