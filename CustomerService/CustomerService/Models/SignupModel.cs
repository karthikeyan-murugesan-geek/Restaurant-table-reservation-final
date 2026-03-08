using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models
{
	public class SignupModel : LoginModel
	{
        [Required]
        public string Role { get; set; }
        public string? MobileNumebr { get; set; }
    }
}

