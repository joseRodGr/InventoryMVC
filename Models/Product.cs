using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ammount { get; set; } = 0;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductSupplier> ProductSuppliers { get; set; }
        public ICollection<InventoryMovement> InventoryMovements { get; set; }

    }
}
