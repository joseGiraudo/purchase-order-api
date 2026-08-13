using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurchaseOrders.Domain.Entities;
using PurchaseOrders.Domain.Interfaces;
using PurchaseOrders.Infrastructure.Persistence;

namespace PurchaseOrders.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Supervisor)
                .Where(u => u.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Supervisor)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Supervisor)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(User user)
        {
            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
