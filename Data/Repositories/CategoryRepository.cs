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
    public class CategoryRepository : GenericRepository<Category>, IRepository<Category, CategoryViewModel>
    {
        private readonly IMapper _mapper;

        public CategoryRepository(InventoryContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PaginatedList<CategoryViewModel>> GetAllPagedAsync(PaginationParams paginationParams)
        {
            var source = _dbSet.AsNoTracking()
                .ProjectTo<CategoryViewModel>(_mapper.ConfigurationProvider);

            return await PaginatedList<CategoryViewModel>.CreateAsync(source, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
