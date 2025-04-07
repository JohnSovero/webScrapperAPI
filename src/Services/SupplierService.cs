using backend.Exceptions;
using backend.Infraestructure;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services{
    public class SupplierService(AppDbContext context) : ISupplierService{
        private readonly AppDbContext _context = context;

        // Method that retrieves all suppliers from the database
        public async Task<IEnumerable<Supplier>> GetAllSuppliers(){   
            return await _context.Suppliers.ToListAsync();
        }

        // Method that retrieves a supplier by its ID
        public async Task<Supplier?> GetSupplierById(Guid id){
            var supplier = await _context.Suppliers.FindAsync(id);
            return supplier == null ? throw new ResourceNotFoundException("Proveedor no encontrado.") : await _context.Suppliers.FindAsync(id);
        }

        // Method to create a new supplier
        public async Task<Supplier> CreateSupplier(SupplierDto dto){
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(p => p.TaxId == dto.TaxId);

            if (existingSupplier != null) 
                throw new ResourceDuplicateException("Ya existe un proveedor con el mismo TaxId.");
            
            existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(p => p.BusinessName == dto.BusinessName);
            if (existingSupplier != null)
                throw new ResourceDuplicateException("Ya existe un proveedor con el mismo BusinessName.");
            
            var supplier = new Supplier{
                Id = Guid.NewGuid(),
                BusinessName = dto.BusinessName,
                TradeName = dto.TradeName,
                TaxId = dto.TaxId,
                Phone = dto.Phone,
                Email = dto.Email,
                Website = dto.Website,
                Address = dto.Address,
                Country = dto.Country,
                AnnualBilling = dto.AnnualBilling,
                LastEdited = DateTime.UtcNow
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        // Method to update an existing supplier
        public async Task<Supplier?> UpdateSupplier(Guid id, SupplierDto dto){
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return null;

            var existingSupplier = await _context.Suppliers
                .FirstOrDefaultAsync(p => (p.TaxId == dto.TaxId || p.BusinessName == dto.BusinessName) && p.Id != id);

            if (existingSupplier != null)
                throw new ResourceDuplicateException("Ya existe un proveedor con el mismo TaxId o BusinessName.");

            supplier.BusinessName = dto.BusinessName;
            supplier.TradeName = dto.TradeName;
            supplier.TaxId = dto.TaxId;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.Website = dto.Website;
            supplier.Address = dto.Address;
            supplier.Country = dto.Country;
            supplier.AnnualBilling = dto.AnnualBilling;
            supplier.LastEdited = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return supplier;
        }

        // Method to delete a supplier by its ID
        public async Task<bool> DeleteSupplier(Guid id)
        {
            var supplier = await _context.Suppliers.FindAsync(id) ?? throw new ResourceNotFoundException("Proveedor no encontrado.");
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
