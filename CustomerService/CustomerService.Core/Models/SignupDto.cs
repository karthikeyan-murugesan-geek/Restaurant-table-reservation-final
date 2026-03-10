using System;
using System.ComponentModel.DataAnnotations;
using CustomerService.Core.Enums;

namespace CustomerService.Core.Models
{
	public class SignupDto : LoginDto
	{
        [Required]
        public UserRole Role { get; set; }
        public string? MobileNumber { get; set; }
    }
}

