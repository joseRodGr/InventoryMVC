using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Interfaces
{
    public interface IStockRepository : IGenericRepository<InventoryMovement>
    {
        Task<IEnumerable<ProductStockViewModel>> GetProductsStockAsync(string searchString);
        Task<IEnumerable<InventoryMovementViewModel>> GetStockMovements(int productId, string typeString);
        Task<int> GetCurrentStockById(int productId);
     
    }
}
