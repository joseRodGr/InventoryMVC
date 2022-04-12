using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
