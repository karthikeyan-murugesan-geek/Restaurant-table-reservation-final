using System;
using ReservationService.Infrastructure.Enum;
using ReservationService.Infrastructure.Models;

namespace ReservationService.Infrastructure.Repositories.Interfaces
{
	public interface IReservationRepository
	{
		Task<Reservation> AddAsync(Reservation reservation);
        Task<Reservation?> UpdateAsync(Reservation reservation);
        Task<bool> DeleteAsync(long id);
        Task<List<Reservation>?> GetByUserAsync(long userID);
        Task<Reservation?> GetAsync(long reservationID);
        Task<List<Reservation>?> GetAllAsync(int? skip = null, int? take = null);
        Task<Reservation?> GetByTableDateSlotAsync(long tableID, DateOnly reservationDate, TimeOnly timeslot);
        Task<List<Reservation>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null);
        Task<Reservation?> UpdateStatusAsync(long id, ReservationStatus status);
    }
}

