using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMVC.Interfaces;
using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Data.Repositories
{
    public class ProductSupplierRepository : IProductSupplierRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public ProductSupplierRepository(InventoryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(ProductSupplier productSupplier)
        {
            _context.ProductSuppliers.Add(productSupplier);
        }

        public void Delete(ProductSupplier productSupplier)
        {
            _context.ProductSuppliers.Remove(productSupplier);
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsBySupplierId(int supplierId)
        {
            return await _context.ProductSuppliers
                        .Where(x => x.SupplierId == supplierId)
                        .Select(x => x.Product)
                        .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                        .ToListAsync();
        }

        public async Task<ProductSupplier> GetProductSupplier(int productId, int supplierId)
        {
            return await _context.ProductSuppliers
                .SingleOrDefaultAsync(x => x.ProductId == productId && x.SupplierId == supplierId);
        }

        public async Task<IEnumerable<SupplierViewModel>> GetSuppliersByProductId(int productId)
        {
            return await _context.ProductSuppliers
                .Where(x => x.ProductId == productId)
                .Select(x => x.Supplier)
                .ProjectTo<SupplierViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
