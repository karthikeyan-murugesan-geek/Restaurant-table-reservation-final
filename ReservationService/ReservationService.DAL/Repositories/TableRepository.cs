using System;
using Microsoft.EntityFrameworkCore;
using ReservationService.DAL.Models;
using ReservationService.DAL.Repositories.Interfaces;

namespace ReservationService.DAL.Repositories
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

