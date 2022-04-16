using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMVC.Helpers;
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
    public class ProductRepository : GenericRepository<Product>, IRepository<Product, ProductViewModel>
    {
        private readonly IMapper _mapper;

        public ProductRepository(InventoryContext context, IMapper mapper) : base(context) 
        {
            _mapper = mapper;
        }
    
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.Category).ToListAsync();
        }

        public async Task<PaginatedList<ProductViewModel>> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var source = _dbSet.AsNoTracking().ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
            return await PaginatedList<ProductViewModel>.CreateAsync(source, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await  _dbSet.Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.Id == id);
       
        }

    }
}
