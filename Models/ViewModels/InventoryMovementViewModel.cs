using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Models.ViewModels
{
  
    public class InventoryMovementViewModel
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Type { get; set; }

        public int Ammount { get; set; }

        [Display(Name ="Product")]
        public string ProductName { get; set; }
    }
}
