using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
