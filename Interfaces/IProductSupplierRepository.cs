using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Interfaces
{
    public interface IProductSupplierRepository
    {
        Task<IEnumerable<SupplierViewModel>> GetSuppliersByProductId(int productId);
        Task<IEnumerable<ProductViewModel>> GetProductsBySupplierId(int supplierId);
        Task<ProductSupplier> GetProductSupplier(int productId, int supplierId);
        void Add(ProductSupplier productSupplier);
        void Delete(ProductSupplier productSupplier);
    }
}
