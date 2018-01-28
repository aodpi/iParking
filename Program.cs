using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iParking.Data;
using iParking.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iParking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).MigrateDbContext<ApplicationDbContext>((context, provider) =>
            {
                var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

                var newUser = new ApplicationUser
                {
                    FirstName = "Adrian",
                    LastName = "Porcescu",
                    PhoneNumber = "074847834",
                    BirthDate = new DateTime(1996, 10, 21),
                    Gender = "Male",
                    UserName = "alloweed",
                    Email = "adrian.porcescu1@gmail.com"
                };

                if (!context.Users.Any())
                {
                    var result = userManager.CreateAsync(newUser, "1234").Result;

                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            Debug.WriteLine(item.Description);
                        }
                    }
                }

                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!context.Roles.Any())
                {


                    roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "Admin",
                    }).Wait();

                    roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "User"
                    }).Wait();

                    var result = userManager.AddToRoleAsync(newUser, "Admin").Result;

                }
            }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
