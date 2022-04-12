using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
    public class EditSupplierViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
    }
}
