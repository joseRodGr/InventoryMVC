using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Product, ProductViewModel> ProductRepository { get;}
        IRepository<Category, CategoryViewModel> CategoryRepository { get;}
        IRepository<Supplier, SupplierViewModel> SupplierRepository { get;}
        IProductSupplierRepository ProductSupplierRepository { get; }
        Task<bool> SaveAllAsync();

    }
}
