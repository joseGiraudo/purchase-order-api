using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;
using PurchaseOrders.Domain.Entities;
using PurchaseOrders.Domain.Enums;
using PurchaseOrders.Domain.Interfaces;

namespace PurchaseOrders.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> CreateAsync(CreateUserDto dto)
        {
            // si viene SupervisorId, validar que exista
            if (dto.SupervisorId.HasValue)
            {
                var supervisor = await _userRepository.GetByIdAsync(dto.SupervisorId.Value);
                if (supervisor is null) return null;
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Enum.Parse<UserRole>(dto.Role),
                SupervisorId = dto.SupervisorId
            };

            var created = await _userRepository.AddAsync(user);
            var userWithSupervisor = await _userRepository.GetByIdAsync(created.Id);

            return ToDto(userWithSupervisor!);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(ToDto).ToList();
        }


        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : ToDto(user);
        }
        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : ToDto(user);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if(user == null) return false;

            if (dto.SupervisorId.HasValue)
            {
                var supervisor = await _userRepository.GetByIdAsync(dto.SupervisorId.Value);
                if (supervisor is null) return false;
            }

            user.Name = dto.Name;
            user.SupervisorId = dto.SupervisorId;
            user.Role = Enum.Parse<UserRole>(dto.Role);
            await _userRepository.UpdateAsync(user);
            return true;
        }



        // metodos privados 
        private static UserDto ToDto(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            SupervisorId = user.SupervisorId,
            SupervisorName = user.Supervisor?.Name // el ?. evita el NullReferenceException si no tiene supervisor
        };



    }
}
