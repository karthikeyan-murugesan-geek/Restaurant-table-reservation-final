using System;
using ReservationService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.Infrastructure;
public class ReservationContext : DbContext
{
    public ReservationContext(DbContextOptions<ReservationContext> options)
        : base(options) { }

    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

}

