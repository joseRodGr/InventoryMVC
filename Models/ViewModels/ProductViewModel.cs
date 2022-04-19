using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Stock")]
        public int Ammount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name ="Category")]
        public string CategoryName { get; set; }


    }
}
