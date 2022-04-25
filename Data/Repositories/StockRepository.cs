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
    public class StockRepository : GenericRepository<InventoryMovement>, IStockRepository
    {
        private readonly InventoryContext _context;
        private readonly IMapper _mapper;

        public StockRepository(InventoryContext context, IMapper mapper): base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductStockViewModel>> GetProductsStockAsync(string searchString)
        {
            //Option 1

            //return await _context.InventoryMovements.AsNoTracking()
            //    .ProjectTo<InventoryMovementViewModel>(_mapper.ConfigurationProvider)
            //    .GroupBy(x => new { x.ProductName, x.ProductId })
            //    .Select(x => new ProductStockViewModel
            //    {
            //        Id = x.Key.ProductId,
            //        Name = x.Key.ProductName,
            //        Stock = x.Sum(x => x.Ammount)
            //    }).ToListAsync();

            //Option 2

            var source = from products in _context.Products select products;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                source = source.Where(x => x.Name.Contains(searchString));
            }

            return await (from products in source
                          join movements in _context.InventoryMovements
                          on products.Id equals movements.ProductId into gj
                          from stock in gj.DefaultIfEmpty()
                          group stock by new
                          {
                              products.Id,
                              products.Name

                          } into gpm
                          select new ProductStockViewModel
                          {
                              Id = gpm.Key.Id,
                              Name = gpm.Key.Name,
                              Stock = gpm.Sum(x => x.Ammount)

                          }).ToListAsync();

            //Option 3

            //return await _context.Products
            //            .GroupJoin(
            //                _context.InventoryMovements,
            //                product => product.Id,
            //                movement => movement.ProductId,
            //                (l, r) => new { Products = l, Movements = r })
            //            .SelectMany(
            //                lr => lr.Movements.DefaultIfEmpty(),
            //                (l, r) => new { Products = l.Products, Movements = r }
            //            ).GroupBy(lr => new
            //            {
            //                lr.Products.Id,
            //                lr.Products.Name

            //            }).Select(gpm => new ProductStockViewModel
            //            {
            //                Id = gpm.Key.Id,
            //                Name = gpm.Key.Name,
            //                Stock = gpm.Sum(x => x.Movements.Ammount)

            //            }).ToListAsync();



        }

        public async Task<int> GetCurrentStockById(int productId)
        {
            return await _context.InventoryMovements
                .Where(x => x.ProductId == productId)
                .SumAsync(x => x.Ammount);
        }

        public async Task<IEnumerable<InventoryMovementViewModel>> GetStockMovements(int productId, string typeString)
        {
            var source = _context.InventoryMovements.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(typeString) && typeString != "All")
            {
                source = source.Where(x => x.Type == typeString);
            }

            return await source
                .Where(x => x.ProductId == productId)
                .ProjectTo<InventoryMovementViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public override async Task<InventoryMovement> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
