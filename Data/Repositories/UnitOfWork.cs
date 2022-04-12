using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using System.Threading.Tasks;

namespace InventoryMVC.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly InventoryContext _context;

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public UnitOfWork(InventoryContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> ProductRepository => _productRepository ?? new ProductRepository(_context);

        public IGenericRepository<Category> CategoryRepository => _categoryRepository ?? new GenericRepository<Category>(_context);

        public IGenericRepository<Supplier> SupplierRepository => _supplierRepository ?? new GenericRepository<Supplier>(_context);

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
