using System;
using ReservationService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.DAL;
public class ReservationContext : DbContext
{
    public ReservationContext(DbContextOptions<ReservationContext> options)
        : base(options) { }

    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

}

