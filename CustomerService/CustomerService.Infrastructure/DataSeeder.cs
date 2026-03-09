using System;
using CustomerService.Infrastructure.Models;

namespace CustomerService.Infrastructure
{
    public static class DataSeeder
    {
        public static void Seed(CustomerContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { ID = 1, Name = "Manager" },
                    new Role { ID = 2, Name = "RestaurantOwner" },
                    new Role { ID = 3, Name = "Customer" }
                );

                context.SaveChanges();
            }
        }
    }
}

