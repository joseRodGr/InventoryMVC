using InventoryMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRoles(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var newUsers = new AppUser[]
            {
                new AppUser {UserName = "user1@email.com", Email = "user1@email.com"},
                new AppUser {UserName = "user2@email.com", Email = "user2@email.com"},
                new AppUser {UserName = "user3@email.com", Email = "user3@email.com"}
       
            };

            var newRoles = new AppRole[]
            {
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Moderator"},
                new AppRole {Name = "User"}
            };

            foreach(var role in newRoles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach(var user in newUsers)
            {
                await userManager.CreateAsync(user, "Abcd*1234");
                await userManager.AddToRoleAsync(user, "User");
            }

            var admin = new AppUser { 
                UserName = "admin@email.com", 
                Email = "admin@email.com" 
            };

            await userManager.CreateAsync(admin, "Abcd*1234");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});

        }
    }
}
