using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Extensions
{
    public static class DataSeeder
    {
        public static async Task SeedData(RepositoryContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            SeedRoles(roleManager);
            SeedUsers(userManager);

        }


        public static void SeedUsers(UserManager<User> userManager)
        {
            try
            {
                if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
                {
                    var user = new User();
                    user.UserName = "admin@gmail.com";
                    user.Email = "admin@gmail.com";

                    IdentityResult result = userManager.CreateAsync(user, "Bs@123").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "admin").Wait();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }



        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("admin").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "admin";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;

                }
                if (!roleManager.RoleExistsAsync("customer").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "customer";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
