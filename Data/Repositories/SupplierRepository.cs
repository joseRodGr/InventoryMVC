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
    public class SupplierRepository : GenericRepository<Supplier>, IRepository<Supplier, SupplierViewModel>
    {
        private readonly IMapper _mapper;

        public SupplierRepository(InventoryContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public Task<PaginatedList<SupplierViewModel>> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var source = _dbSet.AsNoTracking()
                .ProjectTo<SupplierViewModel>(_mapper.ConfigurationProvider);

            return PaginatedList<SupplierViewModel>.CreateAsync(source, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
