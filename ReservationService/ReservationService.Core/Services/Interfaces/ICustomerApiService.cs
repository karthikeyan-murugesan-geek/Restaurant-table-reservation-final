using System;
using ReservationService.Infrastructure.Enum;
using ReservationService.Infrastructure.Models;
using ReservationService.WrapperModels;

namespace ReservationService.Core.Services.Interfaces
{
	public interface ICustomerApiService
	{
        Task<bool> CustomerExists(long userID);
    }
}

