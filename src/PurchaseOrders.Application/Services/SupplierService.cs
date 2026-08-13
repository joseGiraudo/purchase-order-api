using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PurchaseOrders.Application.Dtos;
using PurchaseOrders.Application.Interfaces;
using PurchaseOrders.Domain.Entities;
using PurchaseOrders.Domain.Interfaces;

namespace PurchaseOrders.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        // Metodos publicos
        public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                TaxId = dto.TaxId,
                ContactName = dto.ContactName,
                Email = dto.Email
            };

            var created = await _repository.AddAsync(supplier);
            return ToDto(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier is null) return false;

            await _repository.DeleteAsync(supplier);
            return true;
        }

        public async Task<List<SupplierDto>> GetAllAsync()
        {
            var suppliers = await _repository.GetAllAsync();
            return suppliers.Select(ToDto).ToList();
        }

        public async Task<SupplierDto?> GetByIdAsync(int id)
        {
            var supplier = await _repository.GetByIdAsync(id);
            return supplier is null ? null : ToDto(supplier);
        }

        public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
        {
            var supplier = await _repository.GetByIdAsync(id);
            if (supplier is null) return false;

            supplier.Name = dto.Name;
            supplier.TaxId = dto.TaxId;
            supplier.ContactName = dto.ContactName;
            supplier.Email = dto.Email;

            await _repository.UpdateAsync(supplier);
            return true;
        }



        // metodos privados
        private static SupplierDto ToDto(Supplier supplier) => new()
        {
            Id = supplier.Id,
            Name = supplier.Name,
            TaxId = supplier.TaxId,
            ContactName = supplier.ContactName,
            Email = supplier.Email
        };
    }
}
