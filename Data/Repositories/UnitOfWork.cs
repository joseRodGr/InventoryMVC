using AutoMapper;
using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using System.Threading.Tasks;

namespace InventoryMVC.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly IProductSupplierRepository _productSupplierRepository;

        public UnitOfWork(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IGenericRepository<Product> ProductRepository => _productRepository ?? new ProductRepository(_context);

        public IGenericRepository<Category> CategoryRepository => _categoryRepository ?? new GenericRepository<Category>(_context);

        public IGenericRepository<Supplier> SupplierRepository => _supplierRepository ?? new GenericRepository<Supplier>(_context);

        public IProductSupplierRepository ProductSupplierRepository => _productSupplierRepository ?? new ProductSupplierRepository(_context, _mapper);

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
