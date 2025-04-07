
using backend.Models;

namespace backend.Services{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllSuppliers();
        Task<Supplier?> GetSupplierById(Guid id);
        Task<Supplier> CreateSupplier(SupplierDto dto);
        Task<Supplier?> UpdateSupplier(Guid id, SupplierDto dto);
        Task<bool> DeleteSupplier(Guid id);
    }
}