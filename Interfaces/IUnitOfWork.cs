using InventoryMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> ProductRepository { get;}
        IGenericRepository<Category> CategoryRepository { get;}
        IGenericRepository<Supplier> SupplierRepository { get;}
        IProductSupplierRepository ProductSupplierRepository { get; }
        Task<bool> SaveAllAsync();

    }
}
