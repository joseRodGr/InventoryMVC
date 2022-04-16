using InventoryMVC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Interfaces
{
    public interface IRepository<T, K> : IGenericRepository<T> where K: class where T: class
    {
        Task<PaginatedList<K>> GetAllPagedAsync(PaginationParams paginationParams);
    }
}
