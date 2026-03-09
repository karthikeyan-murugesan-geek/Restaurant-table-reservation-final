using System;
using ReservationService.Infrastructure.Enum;
using ReservationService.WrapperModels.Core;

namespace ReservationService.Core.Services.Interfaces
{
	public interface IReservationService
	{
        Task<ResponseDto<ReservationDto>> CreateReservationAsync(ReservationDto reservationDto);
        Task<ReservationDto?> UpdateAsync(ReservationDto reservationDto);
        Task<bool> DeleteAsync(long reservationID);
        Task<List<ReservationDto>?> GetByUserAsync(long userID);
        Task<ReservationDto?> GetAsync(long reservationID);
        Task<List<ReservationDto>?> GetAllAsync(int? skip = null, int? take = null);
        Task<List<ReservationDto>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null);
        Task<ReservationDto?> UpdateStatusAsync(long id, ReservationStatus status);
    }
}

