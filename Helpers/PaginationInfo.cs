using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Helpers
{
    public class PaginationInfo
    {
        public bool PreviousDisabled { get; set; }
        public bool NextDisabled { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
