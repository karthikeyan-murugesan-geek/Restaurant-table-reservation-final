using System;
namespace ReservationService.Core.WrapperModels
{
	public class UserClaimsDto
	{
		public string? UserID { get; set; }
		public string? UserName { get; set; }
		public List<string>? Roles { get; set; } 
	}
}

