using System;
namespace CustomerService.Models
{
	public class SignupModel : LoginModel
	{
        public string Role { get; set; }
        public string MobileNumebr { get; set; }
    }
}

