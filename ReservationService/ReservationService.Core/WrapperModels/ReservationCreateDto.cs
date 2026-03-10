using System;
namespace ReservationService.Core.WrapperModels
{
    public class ReservationCreateDto
    {
        public int TableID { get; set; }
        public int ReservedByUserId { get; set; }
        public string ReservationDate { get; set; }
        public string TimeSlot { get; set; }
        public int GuestsCount { get; set; }
    }
}

