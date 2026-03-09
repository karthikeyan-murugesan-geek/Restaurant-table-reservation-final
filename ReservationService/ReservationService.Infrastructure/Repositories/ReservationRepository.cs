using System;
using Microsoft.EntityFrameworkCore;
using ReservationService.Infrastructure;
using ReservationService.Infrastructure.Enum;
using ReservationService.Infrastructure.Models;
using ReservationService.Infrastructure.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ReservationService.Infrastructure.Repositories
{
	public class ReservationRepository : IReservationRepository
	{
        private ReservationContext reservationContext;
        public ReservationRepository(ReservationContext reservationContext)
        {
            this.reservationContext = reservationContext;
        }

        public async Task<Reservation?> GetAsync(long reservationID)
        {
            return await reservationContext.Reservations
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.ID == reservationID);
        }

        public async Task<List<Reservation>?> GetByUserAsync(long userID)
        {
            return await reservationContext.Reservations
                .Include(r => r.Table)
                .Where(r => r.ReservedByUserID == userID).ToListAsync();
        }


        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            await reservationContext.Reservations.AddAsync(reservation);
            await reservationContext.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation?> UpdateAsync(Reservation reservation)
        {
            var existing = await reservationContext.Reservations.FindAsync(reservation.ID);
            if (existing == null)
                return null;

            existing.TableID = reservation.TableID;
            existing.ReservedByUserID = reservation.ReservedByUserID;
            existing.ReservationDate = reservation.ReservationDate;
            existing.TimeSlot = reservation.TimeSlot;
            existing.GuestsCount = reservation.GuestsCount;
            existing.ReservationStatus = reservation.ReservationStatus;
            existing.UpdatedAt = System.DateTime.UtcNow;

            await reservationContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var reservation = await reservationContext.Reservations.FindAsync(id);
            if (reservation == null)
                return false;

            reservationContext.Reservations.Remove(reservation);
            await reservationContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Reservation>?> GetAllAsync(int? skip = null, int? take = null)
        {
            var query = reservationContext.Reservations
                .Include(r => r.Table)
                .AsQueryable();

            if (skip.HasValue && take.HasValue)
            {
                query = query.Skip(skip.Value).Take(take.Value);
            }

            return await query.ToListAsync();

        }
        public async Task<Reservation?> GetByTableDateSlotAsync(long tableID, DateOnly reservationDate, TimeOnly timeslot)
        {
            var result = await reservationContext.Reservations
                .FirstOrDefaultAsync(x => x.TableID == tableID && x.ReservationDate == reservationDate
                && x.TimeSlot == timeslot);
            return result;
        }
        public async Task<List<Reservation>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null)
        {
            var reservationsQuery = reservationContext.Reservations
                .Include(x => x.Table)
                .Where(x => x.ReservationDate == reservationDate
                && x.TimeSlot == timeslot)
                .AsQueryable();

            if (skip.HasValue && take.HasValue)
            {
                reservationsQuery = reservationsQuery.Skip(skip.Value).Take(take.Value);
            }

            return await reservationsQuery.ToListAsync();
        }

        public async Task<Reservation?> UpdateStatusAsync(long id, ReservationStatus status)
        {
            var existing = await reservationContext.Reservations.FindAsync(id);
            if (existing == null) return null;

            existing.ReservationStatus = status;
            existing.UpdatedAt = DateTime.UtcNow;

            await reservationContext.SaveChangesAsync();
            return existing;
        }
    }
}

