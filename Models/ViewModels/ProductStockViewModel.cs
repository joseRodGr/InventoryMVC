using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class ProductStockViewModel
    {
        [Display(Name = "Code")]
        public int Id { get; set; }

        [Display(Name="Product")]
        public string Name { get; set; }
        public int Stock { get; set; }
    }
}
