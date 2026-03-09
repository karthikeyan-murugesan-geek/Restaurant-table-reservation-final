using System;
using Microsoft.EntityFrameworkCore;
using ReservationService.Infrastructure.Models;
using ReservationService.Infrastructure.Repositories.Interfaces;

namespace ReservationService.Infrastructure.Repositories
{
	public class TableRepository : ITableRepository
    {
        private ReservationContext reservationContext;
        public TableRepository(ReservationContext reservationContext)
        {
            this.reservationContext = reservationContext;
        }

        public async Task<Table?> GetAsync(long tableID)
        {
            return await reservationContext.Tables
                .FirstOrDefaultAsync(r => r.ID == tableID);
        }
    }
}

