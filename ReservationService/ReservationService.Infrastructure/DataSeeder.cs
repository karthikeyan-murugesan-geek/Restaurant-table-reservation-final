using System;
using ReservationService.Infrastructure.Models;

namespace ReservationService.Infrastructure
{
    public static class DataSeeder
    {
        public static void Seed(ReservationContext context)
        {
            if (!context.Tables.Any())
            {
                context.Tables.AddRange(
                    new Table { ID = 1, Name = "Alpha", Capacity = 3 },
                    new Table { ID = 2, Name = "Beta", Capacity = 4 },
                    new Table { ID = 3, Name = "Gamma", Capacity = 5 },
                    new Table { ID = 4, Name = "Delta", Capacity = 8 },
                    new Table { ID = 5, Name = "Zeta", Capacity = 10 }
                );

                context.SaveChanges();
            }
        }
    }
}

