using System;
using ReservationService.DAL.Enum;
using ReservationService.DAL.Models;
using ReservationService.ViewModel;
using ReservationService.WrapperModels;

namespace ReservationService.Services.Interfaces
{
	public interface IReservationService
	{
        Task<ResponseViewModel<ReservationViewModel>> CreateReservationAsync(ReservationViewModel reservationViewModel);
        Task<ReservationViewModel?> UpdateAsync(ReservationViewModel reservationViewModel);
        Task<bool> DeleteAsync(long reservationID);
        Task<List<ReservationViewModel>?> GetByUserAsync(long userID);
        Task<ReservationViewModel?> GetAsync(long reservationID);
        Task<List<ReservationViewModel>?> GetAllAsync(int? skip = null, int? take = null);
        Task<List<ReservationViewModel>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null);
        Task<ReservationViewModel?> UpdateStatusAsync(long id, ReservationStatus status);
    }
}

