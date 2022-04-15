using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Type { get; set; }
        public int Ammount { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
    