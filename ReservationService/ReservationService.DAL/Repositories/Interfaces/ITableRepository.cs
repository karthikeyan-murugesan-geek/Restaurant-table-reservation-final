using System;
using ReservationService.DAL.Models;

namespace ReservationService.DAL.Repositories.Interfaces
{
	public interface ITableRepository
	{
        Task<Table?> GetAsync(long reservationID);
    }
}

