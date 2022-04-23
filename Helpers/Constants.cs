using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Helpers
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Moderator = "Moderator";
            public const string User = "User";
        }

        public static class Policies
        {
            public const string RequiredAdmin = "RequireAdmin";
        }
    }
}
