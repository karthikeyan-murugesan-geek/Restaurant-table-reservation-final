using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Core.Models
{
	public class LoginDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}

