using System;
using ReservationService.Infrastructure.Models;

namespace ReservationService.Infrastructure.Repositories.Interfaces
{
	public interface ITableRepository
	{
        Task<Table?> GetAsync(long reservationID);
    }
}

