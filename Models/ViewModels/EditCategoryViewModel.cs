using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class EditCategoryViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Category name")]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
