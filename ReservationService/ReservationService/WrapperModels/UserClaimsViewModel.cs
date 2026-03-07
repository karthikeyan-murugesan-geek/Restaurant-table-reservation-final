using System;
namespace ReservationService.WrapperModels
{
	public class UserClaimsViewModel
	{
		public string? UserID { get; set; }
		public string? UserName { get; set; }
		public List<string>? Roles { get; set; } 
	}
}

