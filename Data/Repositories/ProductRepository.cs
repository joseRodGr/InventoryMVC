using InventoryMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(InventoryContext context) : base(context) { }
    
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.Category).ToListAsync();
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await  _dbSet.Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.Id == id);
       
        }
    }
}
