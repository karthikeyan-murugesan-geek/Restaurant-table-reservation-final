using System;
namespace ReservationService.Core.WrapperModels
{
	public class SearchReservationDto
	{
		public string ReservationDate { get; set; }
		public string TimeSlot { get; set; }
		public int? Skip { get; set; } = null;
		public int? Take { get; set; } = null;
	}
}

