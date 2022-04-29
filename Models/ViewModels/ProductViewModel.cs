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
        public string Description { get; set; }

        //[Display(Name = "Price ($)")]
        //[DataType(DataType.Currency)]
        [Required]
        [DisplayFormat(DataFormatString = "{0: $#,###.00}")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name ="Category")]
        public string CategoryName { get; set; }


    }
}
