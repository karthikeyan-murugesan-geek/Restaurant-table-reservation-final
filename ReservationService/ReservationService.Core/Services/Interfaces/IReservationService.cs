using System;
using ReservationService.Core.WrapperModels;
using ReservationService.Infrastructure.Enum;
using ReservationService.WrapperModels.Core;

namespace ReservationService.Core.Services.Interfaces
{
	public interface IReservationService
	{
        Task<ResponseDto<ReservationCreateDto>> CreateReservationAsync(ReservationCreateDto reservationDto);
        Task<ReservationCreateDto?> UpdateAsync(long id, ReservationCreateDto reservationDto);
        Task<bool> DeleteAsync(long reservationID);
        Task<List<ReservationResponseDto>?> GetByUserAsync(long userID);
        Task<ReservationResponseDto?> GetAsync(long reservationID);
        Task<List<ReservationResponseDto>?> GetAllAsync(int? skip = null, int? take = null);
        Task<List<ReservationResponseDto>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null);
        Task<ReservationResponseDto?> UpdateStatusAsync(long id, ReservationStatus status);
    }
}

