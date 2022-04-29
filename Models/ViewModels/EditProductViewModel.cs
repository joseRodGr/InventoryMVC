using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class EditProductViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        //[Required]
        //[DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression(@"^(\d+([\.,]\d{0,2})?|[\.,]?\d{1,2})$", ErrorMessage = "The input must be a number (max. 2 decimals)")]
        [Display(Name = "Price ($)")]
        public string PriceString { get; set; }

        [Required]
        public string CategoryId { get; set; }
    }
}
