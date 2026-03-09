using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Core.Models
{
	public class SignupDto : LoginDto
	{
        [Required]
        public string Role { get; set; }
        public string? MobileNumber { get; set; }
    }
}

