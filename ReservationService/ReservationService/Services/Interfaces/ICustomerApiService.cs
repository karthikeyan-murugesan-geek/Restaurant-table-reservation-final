using System;
using ReservationService.DAL.Enum;
using ReservationService.DAL.Models;
using ReservationService.ViewModel;
using ReservationService.WrapperModels;

namespace ReservationService.Services.Interfaces
{
	public interface ICustomerApiService
	{
        Task<bool> CustomerExists(long userID);
    }
}

