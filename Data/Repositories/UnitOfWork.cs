using AutoMapper;
using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using System.Threading.Tasks;

namespace InventoryMVC.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IRepository<Product, ProductViewModel> ProductRepository => new ProductRepository(_context, _mapper);

        public IRepository<Category, CategoryViewModel> CategoryRepository => new CategoryRepository(_context, _mapper);

        public IRepository<Supplier, SupplierViewModel> SupplierRepository => new SupplierRepository(_context, _mapper);

        public IProductSupplierRepository ProductSupplierRepository => new ProductSupplierRepository(_context, _mapper);

        public IStockRepository StockRepository => new StockRepository(_context,_mapper);

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
