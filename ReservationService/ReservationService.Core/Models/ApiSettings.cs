using System;
namespace ReservationService.Core.Models
{
	public class ApiSettings
	{
		public Service CustomerService { get; set; }
	}

	public class Service
	{
		public string BaseUrl { get; set; }
	}
}

