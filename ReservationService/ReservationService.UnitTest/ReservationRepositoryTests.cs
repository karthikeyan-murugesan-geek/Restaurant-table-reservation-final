
using Xunit;
using Microsoft.EntityFrameworkCore;
using ReservationService.Infrastructure.Models;
using ReservationService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ReservationService.Infrastructure;

namespace ReservationService.UnitTest
{
    public class ReservationRepositoryTests
    {
        private async Task<ReservationContext> GetInMemoryContext(string testDb)
        {
            var options = new DbContextOptionsBuilder<ReservationContext>()
                .UseInMemoryDatabase(testDb) 
                .Options;
            var context = new ReservationContext(options);
            var repository = new ReservationRepository(context);

            var table = new Table { ID = 1, Name = "Alpha", Capacity = 4 };
            await context.Tables.AddAsync(table);
            await context.Reservations.AddRangeAsync(new List<Reservation>
        {
            new Reservation { TableID = 1, ReservedByUserID = 1, ReservationDate = new DateOnly(2026,03,07), TimeSlot = new TimeOnly(10, 00), GuestsCount = 2 },
            new Reservation { TableID = 1, ReservedByUserID = 2, ReservationDate = new DateOnly(2026,03,07), TimeSlot = new TimeOnly(11, 00), GuestsCount = 2 }
        });
            await context.SaveChangesAsync();
            return context;
        }

        [Fact]
        public async Task AddAsync_ShouldAddReservation()
        {
            // Arrange
            var context = await GetInMemoryContext("testDb1");
            var repository = new ReservationRepository(context);

            var table = new Table { ID = 2, Name = "Alpha", Capacity = 4 };
            await context.Tables.AddAsync(table);
            await context.SaveChangesAsync();

            var reservation = new Reservation
            {
                TableID = 2,
                ReservedByUserID = 1,
                ReservationDate = new DateOnly(2026, 03, 07),
                TimeSlot = new TimeOnly(10, 00),
                GuestsCount = 2
            };

            // Act
            var result = await repository.AddAsync(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, context.Reservations.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllReservations()
        {
            var context = await GetInMemoryContext("testDb2");
            var repository = new ReservationRepository(context);
            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result?.Count);
        }

        [Fact]
        public async Task GetAllAsync_WithPagination_ShouldReturn()
        {
            var context = await GetInMemoryContext("testDb3");
            var repository = new ReservationRepository(context);

            // Act: skip 1, take 1
            var result = await repository.GetAllAsync(1, 1);

            // Assert
            Assert.Equal(1, result?.Count);
            Assert.Equal(2, result[0].ReservedByUserID); // second reservation
        }

        [Fact]
        public async Task GetReservationsByDateSlotAsync_ShouldReturnFilteredReservations()
        {
            var context = await GetInMemoryContext("testDb4");
            var repository = new ReservationRepository(context);

            var reservationDate = new DateOnly(2026, 03, 07);
            var timeSlot = new TimeOnly(10, 0);

            // Act
            var result = await repository.GetReservationsByDateSlotAsync(reservationDate, timeSlot);

            // Assert
            Assert.Single(result);
            Assert.Equal("10:00AM", result[0].TimeSlot.ToString());
        }
    }
}

