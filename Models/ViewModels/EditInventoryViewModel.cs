using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class EditInventoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"\b(Input|Output)\b", ErrorMessage ="Option must be Input or Output")]
        public string Type { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Ammount must be greater than zero")]
        public int Ammount { get; set; }

    }
}
